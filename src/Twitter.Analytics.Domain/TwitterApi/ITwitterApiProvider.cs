using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.TwitterApi.Models;

namespace Twitter.Analytics.Domain.TwitterApi
{
    public interface ITwitterApiProvider
    {
        Task<TweetResponseModel> GetMentionsFromAccount(string username);
        Task<TweetResponseModel> GetRepliesFromAccount(string username);
        Task<TweetResponseModel> GetTweetsFromIds(List<long> ids);
        Task<TweetResponseModel> GetPublishedTweetsFromAccount(string accountId);
    }
}
