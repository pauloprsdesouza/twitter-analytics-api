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
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.TwitterApi;


namespace Twitter.Analytics.Infrastructure.Jobs
{
    public class UpdateTweetsJob : BackgroundService
    {
        private readonly ILogger<ExtractAccountJob> _logger;
        private readonly IServiceProvider _provider;

        public UpdateTweetsJob(ILogger<ExtractAccountJob> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Polling service is starting.");
            var accounts = new List<Account>();
            var tweets = new List<Tweet>();
            var idsToBeIgnored = new List<string>();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var scope = _provider.CreateScope();
                    var accountRepository = scope.ServiceProvider.GetService<IAccountRepository>();
                    var tweetRepository = scope.ServiceProvider.GetService<ITweetRepository>();
                    var twitterApiProvider = scope.ServiceProvider.GetService<ITwitterApiProvider>();
                    var tweetService = scope.ServiceProvider.GetService<ITweetService>();

                    if (!accounts.Any())
                        accounts = await accountRepository.FindAllUnprocessed();

                    if (!tweets.Any())
                    {
                        tweets = await tweetRepository.FindAllTweets(accounts.Select(x => x.Id).Take(10).ToList());
                        // foreach (var tweet in tweets)
                        // {
                        //     var author = accounts.SingleOrDefault(x => x.Id == tweet.AuthorId);
                        //     if (author is not null)
                        //         tweet.CalculateSocialCapitalScore(author);
                        // }
                    }

                    if (idsToBeIgnored.Count != tweets.Count)
                    {
                        var tweetsToBeUpdatedPaged = tweets.Where(x => !idsToBeIgnored.Contains(x.Id)).ToList().Take(100).ToList();
                        var tweetsToBeUpdated = await tweetService.UpdateTweets(tweetsToBeUpdatedPaged);

                        if (tweetsToBeUpdated is not null)
                            idsToBeIgnored.AddRange(tweetsToBeUpdated.Select(x => x.Id));
                    }

                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_EXTRACT_PUBLISHED_TWEETS");
            }
        }
    }
}
