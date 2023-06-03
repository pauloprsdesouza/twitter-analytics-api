using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetService
    {
        Task<List<Tweet>> GetTweetsFromIds(List<long> ids);
        Task<List<Tweet>> CreateFromList(List<Tweet> tweets);
        Task<List<Tweet>> GetMentionsFromAccount(string username);
        Task<List<Tweet>> GetRepliesFromAccount(string username);
        Task<List<Tweet>> GetPublishedTweetsFromAccount(string accountId);
    }
}
