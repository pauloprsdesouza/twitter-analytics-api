using System;
using Amazon.DynamoDBv2.DataModel;
using Twitter.Analytics.Infrastructure.Database.Converters;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels
{
    public class BaseModel
    {
        [DynamoDBHashKey]
        public string PK { get; set; }

        [DynamoDBRangeKey]
        public string SK { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string GSIPK { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey]
        public string GSISK { get; set; }

        [DynamoDBProperty(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        [DynamoDBProperty(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
