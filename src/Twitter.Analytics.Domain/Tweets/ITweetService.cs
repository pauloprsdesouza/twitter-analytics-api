using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetService
    {
        Task<List<Tweet>> GetTweetsByAccount(string accountId);
        Task<List<Tweet>> GetMentionsByAccount(string accountId);
        Task<List<Tweet>> ExtractTweetsFromIds(List<long> ids);
        Task<List<Tweet>> CreateFromList(List<Tweet> tweets);
        Task<List<Tweet>> ExtractMentionsFromAccount(string username);
        Task<List<Tweet>> ExtractReplies(string username);
        Task<List<Tweet>> GetRepliesByAccount(string username);
        Task<List<Tweet>> ExtractPublishedTweetsFromAccount(string accountId);
    }
}
