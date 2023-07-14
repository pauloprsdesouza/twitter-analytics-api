using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Annotations.Models;
using Twitter.Analytics.Domain.Hashtags.Models;
using Twitter.Analytics.Domain.Mentions.Models;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.Urls.Models;

namespace Twitter.Analytics.Domain.Tweets
{
    public interface ITweetService
    {
        Task<List<Tweet>> FindAll();
        Task<List<Tweet>> UpdateScores();
        Task<List<Tweet>> UpdateHashtags(List<HashtagModel> hashtags);
        Task<List<Tweet>> UpdateUrls(List<UrlModel> urls);
        Task<List<Tweet>> UpdateMentions(List<MentionModel> mentions);
        Task<List<Tweet>> UpdateAnnotations(List<AnnotationModel> annotations);
        Task<List<Tweet>> GetTweetsByAccount(string accountId);
        Task<List<Tweet>> GetMentionsByAccount(string accountId);
        Task<List<Tweet>> ExtractTweetsFromIds(List<string> ids);
        Task<List<Tweet>> CreateFromList(List<Tweet> tweets);
        Task<List<Tweet>> ExtractMentionsFromAccount(string username);
        Task<List<Tweet>> ExtractReplies(string username);
        Task<List<Tweet>> GetRepliesByAccount(string username);
        Task<List<Tweet>> ExtractPublishedTweetsFromAccount(string accountId);
        Task<List<Tweet>> UpdateTweets(List<Tweet> tweets);
    }
}
