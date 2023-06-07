using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Tweets
{
    public class MentionKey : BaseKey<TweetModel>
    {
        public MentionKey(string toAccountId, string authorId, string tweetId)
        {
            PK = $"Mention#ToAccountId#{toAccountId}";
            SK = $"AuthorId#{authorId}#TweetId#{tweetId}";
        }

        public MentionKey(string toAccountId)
        {
            PK = $"Mention#ToAccountId#{toAccountId}";
        }
    }
}

