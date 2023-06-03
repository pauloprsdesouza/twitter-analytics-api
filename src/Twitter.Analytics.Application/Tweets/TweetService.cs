using System.Linq;
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
        private readonly ITweetRepository _tweetRepository;
        private readonly IMapper _mapper;

        public TweetService(ITwitterApiProvider twitterApi, ITweetRepository tweetRepository, IMapper mapper)
        {
            _twitterApi = twitterApi;
            _tweetRepository = tweetRepository;
            _mapper = mapper;
        }

        public async Task<List<Tweet>> CreateFromList(List<Tweet> tweets)
        {
            return await _tweetRepository.CreateFromList(tweets);
        }

        public async Task<List<Tweet>> GetMentionsFromAccount(string username)
        {
            var response = await _twitterApi.GetMentionsFromAccount(username);

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> GetPublishedTweetsFromAccount(string accountId)
        {
            var response = await _twitterApi.GetPublishedTweetsFromAccount(accountId);

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> GetRepliesFromAccount(string username)
        {
            var response = await _twitterApi.GetRepliesFromAccount(username);

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> GetTweetsFromIds(List<long> ids)
        {
            var tweetResponseModel = await _twitterApi.GetTweetsFromIds(ids);
            if (tweetResponseModel is null) return null;

            return _mapper.Map<List<Tweet>>(tweetResponseModel.GetTweets()); ;
        }
    }
}
