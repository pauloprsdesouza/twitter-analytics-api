using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.TwitterApi.Models;

namespace Twitter.Analytics.Domain.TwitterApi
{
    public interface ITwitterApiProvider
    {
        Task GetTweetsFromAccount();
        Task GetUsersFromIds();
        Task GetRepliesFromAccount();
        Task<TweetResponseModel> GetTweetsFromIds(List<long> ids);
        Task GetPublishedTweetsFromAccount();
    }
}
