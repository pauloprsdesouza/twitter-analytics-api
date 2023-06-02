using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

        public async Task<Tweet> FindById(string tweetId)
        {
            var primaryKey = new TweetKey(tweetId);
            var tweetModel = await _dbContext.LoadAsync<TweetModel>(primaryKey.PK, primaryKey.SK);

            var tweet = _mapper.Map<Tweet>(tweetModel);

            return tweet;
        }

        public async Task<List<Tweet>> GetByAuthorId(string authorId)
        {
            var primaryKey = new TweetKey();
            var tweetsModel = await new DynamoDbQueryBuilder<TweetModel>(primaryKey, _dbContext)
                                    .AddCondition(nameof(Tweet.AuthorId), QueryOperator.Equal, authorId)
                                    .Build();

            return _mapper.Map<List<Tweet>>(tweetsModel);
        }
    }
}
