using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Infrastructure.Mappers;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class MapperProfileDependency
    {
        public static void AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TweetProfile),
                                   typeof(AccountProfile));
        }
    }
}
