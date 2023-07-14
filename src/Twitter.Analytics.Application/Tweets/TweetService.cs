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
using Twitter.Analytics.Domain.Hashtags.Models;
using Twitter.Analytics.Domain.Urls.Models;
using Twitter.Analytics.Domain.Mentions.Models;
using Twitter.Analytics.Domain.Annotations.Models;
using Twitter.Analytics.Domain.Accounts.Entities;

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

        public async Task<List<Tweet>> UpdateScores()
        {
            var tweets = await _tweetRepository.FindAll();
            var accounts = await _accountRepository.FindAll();

            var tasks = accounts.Select(account => UpdateAccountAndTweets(account, tweets)).ToList();

            await Task.WhenAll(tasks);

            return tweets;
        }

        public async Task<List<Tweet>> FindAll()
        {
            return await _tweetRepository.FindAll();
        }

        private async Task UpdateAccountAndTweets(Account account, List<Tweet> tweets)
        {
            var publishedTweets = tweets.Where(x => x.AuthorId == account.Id).ToList();
            account.Published = publishedTweets;

            account.CalculateInfluenceScore();
            account.CalculateReputation();

            await UpdateTweetsAndAccount(account, publishedTweets);
        }

        private async Task UpdateTweetsAndAccount(Account account, List<Tweet> publishedTweets)
        {
            var tasks = publishedTweets.Select(tweet => CalculateSocialCapitalAndTweetUpdate(tweet, account)).ToList();

            await _accountRepository.Update(account);

            await Task.WhenAll(tasks);
        }

        private async Task CalculateSocialCapitalAndTweetUpdate(Tweet tweet, Account account)
        {
            tweet.CalculateSocialCapitalScore(account);

            await _tweetRepository.Update(tweet);
        }

        public async Task<List<Tweet>> CreateFromList(List<Tweet> tweets)
        {
            var total = tweets.Count;
            var pages = (int)Math.Ceiling(total / (double)100);
            var page = 1;

            for (int i = 0; i < pages; i++)
            {
                var tweetsPaged = tweets.Skip(page == 1 ? page : page * 100).Take(100).ToList();
                var requests = tweetsPaged.Select(x => _textAnalysis.GetTextAnalysis(x.Text));
                var responses = await Task.WhenAll(requests);

                var tweetsProcessed = new List<Tweet>();

                for (int j = 0; j < responses.Count(); j++)
                {
                    var tweet = tweetsPaged[j];
                    var response = responses[j];

                    tweet.SentimentScore = response.SentimentScore;
                    tweet.ContextScore = response.ContextScore;
                    tweet.DiversityScore = response.DiversityScore;
                    tweet.Tokens = response.Tokens.Distinct().ToList();

                    tweetsProcessed.Add(tweet);
                }

                page++;

                await _tweetRepository.CreateFromList(tweetsProcessed);

                tweetsProcessed.Clear();
            }

            return tweets;
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

        public async Task<List<Tweet>> UpdateHashtags(List<HashtagModel> hashtags)
        {
            var tweets = await _tweetRepository.FindAll();

            foreach (var tweet in tweets)
            {
                tweet.Hashtags.AddRange(hashtags.Where(x => x.IdTweet == tweet.Id).Select(x => x.Name).Distinct().ToList());
            }

            await _tweetRepository.UpdateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> UpdateUrls(List<UrlModel> urls)
        {
            var tweets = await _tweetRepository.FindAll();

            foreach (var tweet in tweets)
            {
                tweet.Urls.AddRange(urls.Where(x => x.IdTweet == tweet.Id).Select(x => x.Name).Distinct().ToList());
            }

            await _tweetRepository.UpdateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> UpdateMentions(List<MentionModel> mentions)
        {
            var tweets = await _tweetRepository.FindAll();

            foreach (var tweet in tweets)
            {
                tweet.Mentions.AddRange(mentions.Where(x => x.IdTweet == tweet.Id).Select(x => x.IdUser).Distinct().ToList());
            }

            await _tweetRepository.UpdateFromList(tweets);

            return tweets;
        }

        public async Task<List<Tweet>> UpdateAnnotations(List<AnnotationModel> annotations)
        {
            var tweets = await _tweetRepository.FindAll();

            foreach (var tweet in tweets)
            {
                tweet.Domains.AddRange(annotations.Where(x => x.IdTweet == tweet.Id).Select(x => x.Domain).Distinct().ToList());
                tweet.Entities.AddRange(annotations.Where(x => x.IdTweet == tweet.Id).Select(x => x.Entity).Distinct().ToList());
            }

            await _tweetRepository.UpdateFromList(tweets);

            return tweets;
        }
    }
}
