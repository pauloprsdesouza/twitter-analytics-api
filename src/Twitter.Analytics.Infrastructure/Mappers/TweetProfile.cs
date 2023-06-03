using AutoMapper;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.TwitterApi.Models.Tweets;

namespace Twitter.Analytics.Infrastructure.Mappers
{
    public class TweetProfile : Profile
    {
        public TweetProfile()
        {
            CreateMap<TweetModel, Tweet>()
            .ForMember(dest => dest.RetweetCount, src => src.MapFrom(x => x.PublicMetrics.RetweetCount))
            .ForMember(dest => dest.ReplyCount, src => src.MapFrom(x => x.PublicMetrics.ReplyCount))
            .ForMember(dest => dest.LikeCount, src => src.MapFrom(x => x.PublicMetrics.LikeCount))
            .ForMember(dest => dest.QuoteCount, src => src.MapFrom(x => x.PublicMetrics.QuoteCount))
            .ForMember(dest => dest.ImpressionCount, src => src.MapFrom(x => x.PublicMetrics.ImpressionCount))
            .ReverseMap();

             CreateMap<Twitter.Analytics.Infrastructure.Database.DataModel.Tweets.TweetModel, Tweet>().ReverseMap();
        }
    }
}
