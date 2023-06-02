using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitter.Analytics.Domain.Tweets;

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

            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _provider.CreateScope();
                var tweetService = scope.ServiceProvider.GetService<ITweetService>();

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
