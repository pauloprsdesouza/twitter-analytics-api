using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitter.Analytics.Domain.TwitterApi;
using Twitter.Analytics.Domain.TwitterApi.Models;
using Twitter.Analytics.Infrastructure.TwitterApi.HttpWebClient;
using Twitter.Analytics.Domain.TwitterApi.Models.Users;

namespace Twitter.Analytics.Infrastructure.TwitterApi
{
    public class TwitterApiProvider : ITwitterApiProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<TwitterApiProvider> _logger;

        public TwitterApiProvider(HttpClient client, IOptions<TwitterCredentialOptions> twitterCredential, ILogger<TwitterApiProvider> logger)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", twitterCredential.Value.BearerToken);
            _logger = logger;
        }

        private bool IsAvailable(HttpResponseMessage response)
        {
            var limitRemaining = response.Headers.GetValues("x-rate-limit-remaining").FirstOrDefault();
            var whenWillReset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(response.Headers.GetValues("x-rate-limit-reset").FirstOrDefault()));

            var remainigTime = whenWillReset - DateTimeOffset.UtcNow;

            return remainigTime.Minutes > 1;
        }

        public async Task<TweetResponseModel> GetTweetsFromIds(List<string> ids)
        {
            var param = new QueryBuilder();
            param.Add("ids", string.Join(",", ids));
            param.Add("tweet.fields", "attachments,author_id,context_annotations,created_at,entities,geo,in_reply_to_user_id,lang,public_metrics,reply_settings,source");
            param.Add("user.fields", "created_at,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified");
            param.Add("expansions", "attachments.media_keys,referenced_tweets.id,author_id");

            try
            {
                var response = await _client.GetAsync($"tweets{param}");
                response.EnsureSuccessStatusCode();

                return await response.ReadAsJsonAsync<TweetResponseModel>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_GET_TWEETS");
                return null;
            }
        }

        public async Task<TweetResponseModel> GetPublishedTweetsFromAccount(string accountId)
        {
            var param = new QueryBuilder();
            param.Add("tweet.fields", "attachments,author_id,context_annotations,created_at,entities,geo,in_reply_to_user_id,lang,public_metrics,reply_settings,source");
            param.Add("user.fields", "created_at,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified");
            param.Add("expansions", "attachments.media_keys,referenced_tweets.id,author_id");
            param.Add("max_results", "100");

            try
            {
                var response = await _client.GetAsync($"users/{accountId}/tweets{param}");
                response.EnsureSuccessStatusCode();

                return await response.ReadAsJsonAsync<TweetResponseModel>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_GET_TWEETS");
                return null;
            }
        }

        public async Task<TweetResponseModel> GetMentionsFromAccount(string username)
        {
            var param = new QueryBuilder();
            param.Add("query", $"@{username}");
            param.Add("tweet.fields", "attachments,author_id,context_annotations,created_at,entities,geo,in_reply_to_user_id,lang,public_metrics,reply_settings,source");
            param.Add("user.fields", "created_at,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified");
            param.Add("expansions", "attachments.media_keys,referenced_tweets.id,author_id");
            param.Add("max_results", "100");

            try
            {
                var response = await _client.GetAsync($"tweets/search/recent{param}");
                response.EnsureSuccessStatusCode();

                return await response.ReadAsJsonAsync<TweetResponseModel>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_GET_TWEETS");
                return null;
            }
        }

        public async Task<TweetResponseModel> GetRepliesFromAccount(string username)
        {
            var param = new QueryBuilder();
            param.Add("query", $"to:{username}");
            param.Add("tweet.fields", "attachments,author_id,context_annotations,created_at,entities,geo,in_reply_to_user_id,lang,public_metrics,reply_settings,source");
            param.Add("user.fields", "created_at,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified");
            param.Add("expansions", "attachments.media_keys,referenced_tweets.id,author_id");
            param.Add("max_results", "100");

            try
            {
                var response = await _client.GetAsync($"tweets/search/recent{param}");
                response.EnsureSuccessStatusCode();

                return await response.ReadAsJsonAsync<TweetResponseModel>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_GET_TWEETS");
                return null;
            }
        }

        public async Task<List<UserModel>> GetAccounts(List<string> accountIds)
        {
            var param = new QueryBuilder();
            param.Add("ids", string.Join(",", accountIds));
            param.Add("user.fields", "created_at,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified");

            try
            {
                var response = await _client.GetAsync($"users{param}");
                response.EnsureSuccessStatusCode();

                var data = await response.ReadAsJsonAsync<UserDataModel>();
                if(data is null) return null;

                return data.Data;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "UNABLE_TO_GET_TWEETS");
                return null;
            }
        }
    }
}
