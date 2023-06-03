using System;
using System.Collections.Generic;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Tweets.Entities
{
    public class Tweet
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int ImpressionCount { get; set; }
        public TweetType Type { get; set; }
        public List<string> Urls { get; set; }
        public List<string> Mentions { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public int EngagementScore => RetweetCount + ReplyCount + LikeCount + QuoteCount + ImpressionCount;

        public Account Author { get; set; }
    }
}
