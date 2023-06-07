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
    }
}
