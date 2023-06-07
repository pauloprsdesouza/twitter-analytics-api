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
        public int EngagementScore { get; set; }

        [DynamoDBProperty(typeof(EnumConverter<TweetType>))]
        public TweetType Type { get; set; }
    }
}
