using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twitter.Analytics.Domain.TwitterApi;
using Twitter.Analytics.Domain.TwitterApi.Models;
using Twitter.Analytics.Infrastructure.TwitterApi.HttpWebClient;

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

        public async Task<TweetResponseModel> GetTweetsFromIds(List<long> ids)
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

        public Task GetPublishedTweetsFromAccount()
        {
            throw new NotImplementedException();
        }

        public Task GetRepliesFromAccount()
        {
            throw new NotImplementedException();
        }

        public Task GetTweetsFromAccount()
        {
            throw new NotImplementedException();
        }

        public Task GetUsersFromIds()
        {
            throw new NotImplementedException();
        }
    }
}