using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.TwitterApi;

namespace Twitter.Analytics.Application.Tweets
{
    public class TweetService : ITweetService
    {
        private readonly ITwitterApiProvider _twitterApi;
        private readonly IMapper _mapper;

        public TweetService(ITwitterApiProvider twitterApi, IMapper mapper)
        {
            _twitterApi = twitterApi;
            _mapper = mapper;
        }

        public async Task<List<Tweet>> GetTweetsFromIds(List<long> ids)
        {
            var tweetResponseModel = await _twitterApi.GetTweetsFromIds(ids);
            if (tweetResponseModel is null) return null;

            return _mapper.Map<List<Tweet>>(tweetResponseModel.GetTweets()); ;
        }
    }
}
