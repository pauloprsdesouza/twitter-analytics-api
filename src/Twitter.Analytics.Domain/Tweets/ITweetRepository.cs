using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetRepository
    {
        Task<Tweet> Update(Tweet tweet);
        Task<List<Tweet>> FindAll();
        Task<List<Tweet>> FindAllTweets(List<string> usersId);
        Task<List<Tweet>> GetByAuthorId(string authorId);
        Task<List<Tweet>> GetRepliesByUser(string userId);
        Task<List<Tweet>> GetMentionsByUser(string userId);
        Task<Tweet> Create(Tweet tweet);
        Task<List<Tweet>> CreateFromList(List<Tweet> tweets);
        Task<List<Tweet>> UpdateFromList(List<Tweet> tweets);
        Task<List<Tweet>> CreateMentionsFromList(string toAccountId, List<Tweet> tweets);
        Task<List<Tweet>> CreateRepliesFromList(string toAccountId, List<Tweet> tweets);
    }
}
