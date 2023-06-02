using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Application.Accounts;
using Twitter.Analytics.Application.Tweets;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Tweets;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class DomainServiceDependency
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITweetService, TweetService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
