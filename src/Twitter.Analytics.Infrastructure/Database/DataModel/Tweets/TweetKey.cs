using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    public class TweetKey : BaseKey<TweetModel>
    {
        public TweetKey(string tweetId, string authorId)
        {
            PK = $"Tweet#AuthorId#{authorId}";
            SK = $"TweetId#{tweetId}";
        }

        public TweetKey(string authorId)
        {
            PK = $"Tweet#AuthorId#{authorId}";
        }
    }
}
