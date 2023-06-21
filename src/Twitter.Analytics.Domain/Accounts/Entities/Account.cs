using System;
using System.Collections.Generic;
using System.Linq;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Accounts.Entities
{
    public class Account
    {
        public Account()
        {
            Published = new();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int TweetCount { get; set; }
        public int ListedCount { get; set; }
        public bool Verified { get; set; }
        public bool IsProcessed { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public List<Tweet> Published { get; set; }
        public List<Tweet> Mentions { get; set; }
        public List<Tweet> Replies { get; set; }

        public double RecencyScore => CalculateRecencyScore();
        public double InfluenceScore => CalculateInfluenceScore();
        public double ReputationScore => CalculateReputation();
        public double EngagementStrengthScore => InfluenceScore * ReputationScore;

        private double CalculateRecencyScore()
        {
            var ageInSeconds = DateTimeOffset.UtcNow.Subtract(CreatedAt).TotalSeconds;

            var decayFactor = 0.1;

            return 1 / (1 + decayFactor * Math.Log10(1 + ageInSeconds));
        }

        private double CalculateInfluenceScore()
        {
            if (Published.Any())
            {
                var engagementScore = Published.Sum(x => x.EngagementScore);
                if (engagementScore > 0 && FollowersCount > 0)
                    return engagementScore / (Published.Count * FollowersCount);
            }

            return 0;
        }

        private double CalculateReputation()
        {
            var positiveSentiment = 0.0;
            var negativeSentiment = 0.0;
            var reputationScore = 0.0;

            foreach (var mention in Mentions)
            {
                var result = CalculateSentimentScore(mention);

                positiveSentiment += result.Item1;
                negativeSentiment += result.Item1;
            }

            foreach (var reply in Replies)
            {
                var result = CalculateSentimentScore(reply);

                positiveSentiment += result.Item1;
                negativeSentiment += result.Item1;
            }

            if ((positiveSentiment + negativeSentiment) > 0.0)
                reputationScore = positiveSentiment / (positiveSentiment + negativeSentiment);
            else
                reputationScore = 0.0;

            var fineAdjustment = 0.01;
            var normalizedReputationScore = (reputationScore + ListedCount * fineAdjustment) / (1.0 + (ListedCount * fineAdjustment));

            return normalizedReputationScore;
        }

        private (double, double) CalculateSentimentScore(Tweet tweet)
        {
            var positiveSentiment = 0;
            var negativeSentiment = 0;

            if (tweet.AuthorId != Id)
                switch (tweet.SentimentScore)
                {
                    case > 0:
                        positiveSentiment++;
                        break;
                    case < 0:
                        negativeSentiment++;
                        break;
                }

            return (positiveSentiment, negativeSentiment);
        }
    }
}
