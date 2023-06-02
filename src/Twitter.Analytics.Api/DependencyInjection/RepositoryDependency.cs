using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Infrastructure.Database.DataModel.Tweets;
using Twitter.Analytics.Infrastructure.Database.DataModel.Users;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class RepositoryDependency
    {
         public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITweetRepository, TweetRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
