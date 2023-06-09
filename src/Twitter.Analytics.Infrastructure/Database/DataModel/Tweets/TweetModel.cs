using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Infrastructure.Database.Converters;
using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    [DynamoDBTable(DynamoDbTable.Schema)]
    public class TweetModel : BaseModel
    {
        [DynamoDBProperty]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string AuthorId { get; set; }

        [DynamoDBProperty]
        public string Text { get; set; }

        [DynamoDBProperty]
        public int RetweetCount { get; set; }

        [DynamoDBProperty]
        public int ReplyCount { get; set; }

        [DynamoDBProperty]
        public int LikeCount { get; set; }

        [DynamoDBProperty]
        public int QuoteCount { get; set; }

        [DynamoDBProperty]
        public int ImpressionCount { get; set; }

        [DynamoDBProperty]
        public double EngagementScore { get; set; }

        [DynamoDBProperty(typeof(EnumConverter<TweetType>))]
        public TweetType Type { get; set; }

        [DynamoDBProperty]
        public double ContextScore { get; set; }

        [DynamoDBProperty]
        public double DiversityScore { get; set; }

        [DynamoDBProperty]
        public double SentimentScore { get; set; }

        [DynamoDBProperty]
        public List<string> Tokens { get; set; }

        [DynamoDBProperty]
        public List<string> Urls { get; set; }

        [DynamoDBProperty]
        public List<string> Mentions { get; set; }

        [DynamoDBProperty]
        public List<string> Hashtags { get; set; }

        [DynamoDBProperty]
        public List<string> Domains { get; set; }

        [DynamoDBProperty]
        public List<string> Entities { get; set; }

        [DynamoDBProperty]
        public double RecencyScore { get; set; }

        [DynamoDBProperty]
        public double SocialCapitalScore { get; set; }
    }
}
