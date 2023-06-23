using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.TwitterApi;
using System;
using Twitter.Analytics.Domain.Accounts;
using Microsoft.Extensions.Logging;

namespace Twitter.Analytics.Application.Tweets
{
    public class TweetService : ITweetService
    {
        private readonly ITwitterApiProvider _twitterApi;
        private readonly ITweetRepository _tweetRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TweetService> _logger;

        public TweetService(ITwitterApiProvider twitterApi, ITweetRepository tweetRepository, IMapper mapper, IAccountRepository accountRepository, ILogger<TweetService> logger)
        {
            _twitterApi = twitterApi;
            _tweetRepository = tweetRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<List<Tweet>> CreateFromList(List<Tweet> tweets)
        {
            return await _tweetRepository.CreateFromList(tweets);
        }

        public async Task<List<Tweet>> ExtractReplies(string username)
        {
            var response = await _twitterApi.GetRepliesFromAccount(username);
            if (response is null) return null;

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> ExtractMentionsFromAccount(string username)
        {
            var response = await _twitterApi.GetMentionsFromAccount(username);
            if (response is null) return null;

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> ExtractPublishedTweetsFromAccount(string accountId)
        {
            var response = await _twitterApi.GetPublishedTweetsFromAccount(accountId);
            if (response is null) return null;

            var tweets = _mapper.Map<List<Tweet>>(response.Data);
            if (tweets.Any())
                await _tweetRepository.CreateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> UpdateTweets(List<Tweet> tweets)
        {
            var ids = tweets.Select(x => x.Id).ToList();
            var tweetsExtracted = await ExtractTweetsFromIds(ids);
            if (tweetsExtracted.Any())
            {
                var tweetsToUpdate = _mapper.Map<List<Tweet>>(tweetsExtracted);

                foreach (var tweetExtracted in tweetsToUpdate)
                {
                    var tweetToUpdate = tweets.SingleOrDefault(x => x.Id == tweetExtracted.Id);
                    if (tweetToUpdate is not null)
                    {
                        tweetToUpdate.Id = tweetExtracted.Id;
                        tweetToUpdate.Text = tweetExtracted.Text;
                        tweetToUpdate.AuthorId = tweetExtracted.AuthorId;
                        tweetToUpdate.RetweetCount = tweetExtracted.RetweetCount;
                        tweetToUpdate.ReplyCount = tweetExtracted.ReplyCount;
                        tweetToUpdate.LikeCount = tweetExtracted.LikeCount;
                        tweetToUpdate.QuoteCount = tweetExtracted.QuoteCount;
                        tweetToUpdate.ImpressionCount = tweetExtracted.ImpressionCount;
                        tweetToUpdate.Urls = tweetExtracted.Urls;
                        tweetToUpdate.Mentions = tweetExtracted.Mentions;
                        tweetToUpdate.Hashtags = tweetExtracted.Hashtags;
                        tweetToUpdate.Domains = tweetExtracted.Domains;
                        tweetToUpdate.Entities = tweetExtracted.Entities;
                        tweetToUpdate.CreatedAt = tweetExtracted.CreatedAt;
                    }
                }

                await _tweetRepository.UpdateFromList(tweetsToUpdate);
            }

            return tweetsExtracted;
        }

        public async Task<List<Tweet>> GetRepliesByAccount(string accountId)
        {
            return await _tweetRepository.GetRepliesByUser(accountId);
        }

        public async Task<List<Tweet>> GetMentionsByAccount(string accountId)
        {
            return await _tweetRepository.GetMentionsByUser(accountId);
        }

        public async Task<List<Tweet>> GetTweetsByAccount(string accountId)
        {
            return await _tweetRepository.GetByAuthorId(accountId);
        }

        public async Task<List<Tweet>> ExtractTweetsFromIds(List<string> ids)
        {
            var tweetResponseModel = await _twitterApi.GetTweetsFromIds(ids);
            if (tweetResponseModel is null) return null;

            return _mapper.Map<List<Tweet>>(tweetResponseModel.GetTweets());
        }
    }
}
