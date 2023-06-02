using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetRepository
    {
        Task<List<Tweet>> GetByAuthorId(string authorId);
        Task<Tweet> FindById(string tweetId);
        Task<Tweet> Create(Tweet tweet);
    }
}
