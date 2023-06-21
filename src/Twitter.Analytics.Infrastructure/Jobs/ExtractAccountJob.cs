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
    public class ExtractAccountJob : BackgroundService
    {
        private readonly ILogger<ExtractAccountJob> _logger;
        private readonly IServiceProvider _provider;

        public ExtractAccountJob(ILogger<ExtractAccountJob> logger, IServiceProvider provider)
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

                    if (!accounts.Any())
                        accounts = await accountRepository.FindAllUnprocessed();

                    if (!tweets.Any())
                        tweets = await tweetRepository.FindAllTweets(accounts.Select(x => x.Id).ToList());

                    var accountsToBeExtracted = tweets.Select(x => x.AuthorId).Where(x => !accounts.Select(y => y.Id).Contains(x) && !idsToBeIgnored.Contains(x)).Distinct().ToList();
                    if (accountsToBeExtracted.Any())
                    {
                        var accountsToBeExtractedPaged = accountsToBeExtracted.Take(50).ToList();
                        var accountsSaved = await accountRepository.ExtractAccounts(accountsToBeExtractedPaged);
                        if (accountsSaved is null)
                            idsToBeIgnored.AddRange(accountsToBeExtractedPaged);
                        else
                        {
                            idsToBeIgnored.AddRange(accountsToBeExtractedPaged.Where(x => !accountsSaved.Select(y => y.Id).Contains(x)).ToList());
                            accounts.AddRange(accountsSaved);
                        }
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
