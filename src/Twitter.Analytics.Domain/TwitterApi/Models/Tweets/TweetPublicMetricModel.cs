using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analytics.Domain.TwitterApi.Models.Tweets
{
    public class TweetPublicMetricModel
    {
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int ImpressionCount { get; set; }
    }
}
