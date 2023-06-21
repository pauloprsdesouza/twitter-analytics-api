using System;
using System.Collections.Generic;
using Twitter.Analytics.Domain.TwitterApi.Models.Annotations;
using Twitter.Analytics.Domain.TwitterApi.Models.Entities;
using Twitter.Analytics.Domain.TwitterApi.Models.Users;

namespace Twitter.Analytics.Domain.TwitterApi.Models.Tweets
{
    public class TweetModel
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public UserModel Author { get; set; }
        public int RetweetCount { get; set; }
        public int LikeCount { get; set; }
        public string Lang { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public TweetPublicMetricModel PublicMetrics { get; set; }
        public List<ContextAnnotation> ContextAnnotations { get; set; }
        public EntityDetailModel Entities { get; set; }

        public TweetModel()
        {
            ContextAnnotations = new();
        }
    }
}
