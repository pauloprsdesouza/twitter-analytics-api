using AutoMapper;
using Twitter.Analytics.Domain.Accounts.Entities;

using Twitter.Analytics.Domain.TwitterApi.Models.Users;
using Twitter.Analytics.Infrastructure.Database.DataModel.Users;

namespace Twitter.Analytics.Infrastructure.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<UserModel, Account>()
            .ForMember(dest => dest.FollowersCount, src => src.MapFrom(x => x.PublicMetrics.FollowersCount))
            .ForMember(dest => dest.FollowingCount, src => src.MapFrom(x => x.PublicMetrics.FollowingCount))
            .ForMember(dest => dest.TweetCount, src => src.MapFrom(x => x.PublicMetrics.TweetCount))
            .ForMember(dest => dest.ListedCount, src => src.MapFrom(x => x.PublicMetrics.ListedCount))
            .ReverseMap();

            CreateMap<AccountModel, Account>().ReverseMap();
        }
    }
}
