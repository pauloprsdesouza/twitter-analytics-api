using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Domain.TwitterApi;
using Twitter.Analytics.Domain.TwitterApi.Models;
using Twitter.Analytics.Infrastructure.TwitterApi;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class TwitterApiDependency
    {
        public static void AddTwitterApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwitterCredentialOptions>(configuration);

            services.AddHttpClient<ITwitterApiProvider, TwitterApiProvider>("Twitter", client =>
            {
                client.BaseAddress = new Uri(configuration[nameof(TwitterCredentialOptions.BaseUrl)]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}
