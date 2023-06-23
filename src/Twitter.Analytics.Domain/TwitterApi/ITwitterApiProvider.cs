using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.TwitterApi.Models;
using Twitter.Analytics.Domain.TwitterApi.Models.Users;

namespace Twitter.Analytics.Domain.TwitterApi
{
    public interface ITwitterApiProvider
    {
        Task<TweetResponseModel> GetMentionsFromAccount(string username);
        Task<TweetResponseModel> GetRepliesFromAccount(string username);
        Task<TweetResponseModel> GetTweetsFromIds(List<string> ids);
        Task<TweetResponseModel> GetPublishedTweetsFromAccount(string accountId);
        Task<List<UserModel>> GetAccounts(List<string> accountIds);
    }
}
