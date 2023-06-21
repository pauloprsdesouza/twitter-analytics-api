using System.Linq;
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
            .ForMember(dest => dest.Urls, src => src.MapFrom(x => x.Entities.Urls.Select(y => y.ExpandedUrl)))
            .ForMember(dest => dest.Mentions, src => src.MapFrom(x => x.Entities.Mentions.Select(y => y.Id)))
            .ForMember(dest => dest.Hashtags, src => src.MapFrom(x => x.Entities.Hashtags.Select(y => y.Tag)))
            .ForMember(dest => dest.Domains, src => src.MapFrom(x => x.ContextAnnotations.SelectMany(y => y.Domain.Id)))
            .ForMember(dest => dest.Entities, src => src.MapFrom(x => x.ContextAnnotations.SelectMany(y => y.Entity.Id)))
            .ReverseMap();

            CreateMap<Twitter.Analytics.Infrastructure.Database.DataModel.Tweets.TweetModel, Tweet>().ReverseMap();
        }
    }
}
