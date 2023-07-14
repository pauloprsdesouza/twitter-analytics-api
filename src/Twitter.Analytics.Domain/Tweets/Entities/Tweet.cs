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
        public double ContextScore { get; set; }
        public double DiversityScore { get; set; }
        public double SentimentScore { get; set; }
        public List<string> Tokens { get; set; }
        public List<string> Urls { get; set; }
        public List<string> Mentions { get; set; }
        public List<string> Hashtags { get; set; }
        public List<string> Domains { get; set; }
        public List<string> Entities { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public double EngagementScore => RetweetCount + ReplyCount + LikeCount + QuoteCount + ImpressionCount;
        public double RecencyScore { get; set; }
        public double SocialCapitalScore { get; private set; }

        private double CalculateRecencyScore()
        {
            var ageInSeconds = DateTimeOffset.UtcNow.Subtract(CreatedAt).TotalSeconds;

            var decayFactor = 0.1;

            return 1 / (1 + decayFactor * Math.Log10(1 + ageInSeconds));
        }

        public void CalculateSocialCapitalScore(Account author)
        {
            SocialCapitalScore = (author.EngagementStrengthScore + RetweetCount + EngagementScore + Hashtags.Count + Urls.Count + DiversityScore + ContextScore + Tokens.Count) * RecencyScore;
        }
    }
}
