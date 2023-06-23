using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    public class TweetRepository : ITweetRepository
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;

        public TweetRepository(IDynamoDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Tweet> Create(Tweet tweet)
        {
            var tweetModel = _mapper.Map<TweetModel>(tweet);

            var primaryKey = new TweetKey(tweet.Id);
            primaryKey.AssignTo(tweetModel);

            await _dbContext.SaveAsync(tweetModel);

            return tweet;
        }

        public async Task<List<Tweet>> CreateFromList(List<Tweet> tweets)
        {
            var batch = _dbContext.CreateBatchWrite<TweetModel>();
            var tweetsModel = _mapper.Map<List<TweetModel>>(tweets.DistinctBy(x => x.Id).ToList());

            foreach (var tweet in tweetsModel)
            {
                var primaryKey = new TweetKey(tweet.Id, tweet.AuthorId);
                primaryKey.AssignTo(tweet);
            }

            batch.AddPutItems(tweetsModel);
            await batch.ExecuteAsync();

            return tweets;
        }

        public async Task<List<Tweet>> CreateMentionsFromList(string toAccountId, List<Tweet> tweets)
        {
            var batch = _dbContext.CreateBatchWrite<TweetModel>();
            var tweetsModel = _mapper.Map<List<TweetModel>>(tweets.DistinctBy(x => x.Id).ToList());

            foreach (var tweet in tweetsModel)
            {
                var primaryKey = new MentionKey(toAccountId, tweet.AuthorId, tweet.Id);
                primaryKey.AssignTo(tweet);
            }

            batch.AddPutItems(tweetsModel);
            await batch.ExecuteAsync();

            return tweets;
        }

        public async Task<List<Tweet>> CreateRepliesFromList(string toAccountId, List<Tweet> tweets)
        {
            var batch = _dbContext.CreateBatchWrite<TweetModel>();
            var tweetsModel = _mapper.Map<List<TweetModel>>(tweets.DistinctBy(x => x.Id).ToList());

            foreach (var tweet in tweetsModel)
            {
                var primaryKey = new ReplyKey(toAccountId, tweet.AuthorId, tweet.Id);
                primaryKey.AssignTo(tweet);
            }

            batch.AddPutItems(tweetsModel);
            await batch.ExecuteAsync();

            return tweets;
        }

        public async Task<List<Tweet>> FindAllTweets(List<string> usersId)
        {
            var requests = new List<Task<List<TweetModel>>>();
            foreach (var userId in usersId)
            {
                var mentionKey = new MentionKey(userId);
                requests.Add(new DynamoDbQueryBuilder<TweetModel>(mentionKey, _dbContext).Build());

                var replyKey = new ReplyKey(userId);
                requests.Add(new DynamoDbQueryBuilder<TweetModel>(replyKey, _dbContext).Build());

                var publishedKey = new TweetKey(userId);
                requests.Add(new DynamoDbQueryBuilder<TweetModel>(publishedKey, _dbContext).Build());
            }

            var responses = await Task.WhenAll(requests);

            return _mapper.Map<List<Tweet>>(responses.SelectMany(x => x.ToList()));
        }

        public async Task<List<Tweet>> GetByAuthorId(string authorId)
        {
            var primaryKey = new TweetKey(authorId);
            var tweetsModel = await new DynamoDbQueryBuilder<TweetModel>(primaryKey, _dbContext)
                                    .Build();

            return _mapper.Map<List<Tweet>>(tweetsModel);
        }

        public async Task<List<Tweet>> GetMentionsByUser(string toUserId)
        {
            var primaryKey = new MentionKey(toUserId);
            var tweetsModel = await new DynamoDbQueryBuilder<TweetModel>(primaryKey, _dbContext)
                                    .Build();

            return _mapper.Map<List<Tweet>>(tweetsModel);
        }

        public async Task<List<Tweet>> GetRepliesByUser(string toUserId)
        {
            var primaryKey = new ReplyKey(toUserId);
            var tweetsModel = await new DynamoDbQueryBuilder<TweetModel>(primaryKey, _dbContext)
                                    .Build();

            return _mapper.Map<List<Tweet>>(tweetsModel);
        }

        public async Task<List<Tweet>> UpdateFromList(List<Tweet> tweets)
        {
            var batch = _dbContext.CreateBatchWrite<TweetModel>();
            var tweetsModel = _mapper.Map<List<TweetModel>>(tweets.DistinctBy(x => x.Id).ToList());

            foreach (var tweet in tweetsModel)
            {
                var primaryKey = new TweetKey(tweet.Id, tweet.AuthorId);
                primaryKey.AssignTo(tweet);
            }

            batch.AddPutItems(tweetsModel);
            await batch.ExecuteAsync();

            return tweets;
        }
    }
}
