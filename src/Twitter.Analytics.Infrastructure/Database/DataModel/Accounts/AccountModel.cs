using Amazon.DynamoDBv2.DataModel;
using Twitter.Analytics.Infrastructure.Database.Converters;
using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Users
{
    [DynamoDBTable(DynamoDbTable.Schema)]
    public class AccountModel : BaseModel
    {
        [DynamoDBProperty]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string Username { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public int FollowersCount { get; set; }

        [DynamoDBProperty]
        public int FollowingCount { get; set; }

        [DynamoDBProperty]
        public int TweetCount { get; set; }

        [DynamoDBProperty]
        public int ListedCount { get; set; }

        [DynamoDBProperty]
        public double RecencyScore { get; set; }

        [DynamoDBProperty]
        public double InfluenceScore { get; set; }

        [DynamoDBProperty]
        public double ReputationScore { get; set; }

        [DynamoDBProperty]
        public double EngagementStrengthScore { get; set; }

        [DynamoDBProperty(typeof(BoolConverter))]
        public bool IsProcessed { get; set; }
    }
}
