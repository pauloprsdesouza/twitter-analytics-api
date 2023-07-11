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
using Twitter.Analytics.Domain.TextAnalysisApi;

namespace Twitter.Analytics.Application.Tweets
{
    public class TweetService : ITweetService
    {
        private readonly ITwitterApiProvider _twitterApi;
        private readonly ITweetRepository _tweetRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TweetService> _logger;
        private readonly ITextAnalysisApi _textAnalysis;

        public TweetService(ITwitterApiProvider twitterApi, ITweetRepository tweetRepository, IMapper mapper, IAccountRepository accountRepository, ILogger<TweetService> logger, ITextAnalysisApi textAnalysis)
        {
            _twitterApi = twitterApi;
            _tweetRepository = tweetRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _logger = logger;
            _textAnalysis = textAnalysis;
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
            if (tweetsExtracted is not null)
            {
                var tweetsToUpdate = _mapper.Map<List<Tweet>>(tweetsExtracted);

                foreach (var tweetExtracted in tweetsToUpdate)
                {
                    var tweetToUpdate = tweets.SingleOrDefault(x => x.Id == tweetExtracted.Id);
                    if (tweetToUpdate is not null)
                    {
                        var response = _textAnalysis.GetTextAnalysis(tweetExtracted.Text);
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
                        tweetToUpdate.ContextScore = tweetToUpdate.ContextScore;
                        tweetToUpdate.DiversityScore = tweetToUpdate.DiversityScore;
                        tweetToUpdate.SentimentScore = tweetToUpdate.SentimentScore;
                        tweetToUpdate.Tokens = tweetToUpdate.Tokens;
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
