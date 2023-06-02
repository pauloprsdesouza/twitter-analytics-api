using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    public class TweetKey : BaseKey<TweetModel>
    {
        public TweetKey(string tweetId)
        {
            PK = $"Tweet";
            SK = $"Id#{tweetId}";
        }

        public TweetKey()
        {
            PK = $"Tweet";
        }
    }
}
