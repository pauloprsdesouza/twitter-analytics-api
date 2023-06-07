using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Tweets;
using System.Collections.Generic;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Infrastructure.Jobs
{
    public class ExtractTweetJob : BackgroundService
    {
        private readonly ILogger<ExtractTweetJob> _logger;
        private readonly IServiceProvider _provider;

        public ExtractTweetJob(ILogger<ExtractTweetJob> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling service is starting.");
            var accounts = new List<Account>();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var scope = _provider.CreateScope();
                    var accountRepository = scope.ServiceProvider.GetService<IAccountRepository>();
                    var tweetService = scope.ServiceProvider.GetService<ITweetService>();
                    var tweetRepository = scope.ServiceProvider.GetService<ITweetRepository>();

                    if (!accounts.Any())
                        accounts = await accountRepository.FindAllUnprocessed();

                    var account = accounts.Where(x => x.IsProcessed == false).FirstOrDefault();

                    if (account is not null)
                    {
                        CreatePublishedTweets(account, tweetService, tweetRepository);
                        CreateReplies(account, tweetService, tweetRepository);
                        CreateMentions(account, tweetService, tweetRepository);

                        account.IsProcessed = true;
                        await accountRepository.Update(account);

                        _logger.LogInformation("TWEETS_SAVED_WITH_SUCCESS");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_EXTRACT_PUBLISHED_TWEETS");
            }
        }

        private async void CreatePublishedTweets(Account account, ITweetService tweetService, ITweetRepository tweetRepository)
        {
            var tweets = await tweetService.ExtractPublishedTweetsFromAccount(account.Id);
            if (tweets is not null)
            {
                await tweetRepository.CreateFromList(tweets);
            }
        }

        private async void CreateReplies(Account account, ITweetService tweetService, ITweetRepository tweetRepository)
        {
            var tweets = await tweetService.ExtractReplies(account.Username);
            if (tweets is not null)
            {
                await tweetRepository.CreateRepliesFromList(account.Id, tweets);
            }
        }

        private async void CreateMentions(Account account, ITweetService tweetService, ITweetRepository tweetRepository)
        {
            var tweets = await tweetService.ExtractMentionsFromAccount(account.Username);
            if (tweets is not null)
            {
                await tweetRepository.CreateMentionsFromList(account.Id, tweets);
            }
        }
    }
}
