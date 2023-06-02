using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analytics.Domain.TwitterApi.Models.Users
{
    public class UserPublicMetricModel
    {
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int TweetCount { get; set; }
        public int ListedCount { get; set; }
    }
}
