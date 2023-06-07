using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    public class ReplyKey : BaseKey<TweetModel>
    {
        public ReplyKey(string toAccountId, string authorId, string tweetId)
        {
            PK = $"Reply#ToAccountId#{toAccountId}";
            SK = $"AuthorId#{authorId}#TweetId#{tweetId}";
        }

        public ReplyKey(string toAccountId)
        {
            PK = $"Reply#ToAccountId#{toAccountId}";
        }
    }
}
