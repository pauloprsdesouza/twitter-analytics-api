using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetService
    {
        Task<List<Tweet>> GetTweetsFromIds(List<long> ids);
    }
}
