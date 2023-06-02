using System;

namespace Twitter.Analytics.Domain.TwitterApi.Models.Users
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public bool Protected { get; set; }
        public bool Verified { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public UserPublicMetricModel PublicMetrics { get; set; }
    }
}
