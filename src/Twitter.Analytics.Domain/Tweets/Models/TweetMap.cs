using System.Linq;
using CsvHelper.Configuration;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets.Models
{
    public class TweetMap : ClassMap<Tweet>
    {
        public TweetMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Text).Name("text");
            Map(x => x.AuthorId).Name("author_id");
            Map(x => x.QuoteCount).Name("quote_count");
            Map(x => x.ImpressionCount).Name("impression_count");
            Map(x => x.ReplyCount).Name("reply_count");
            Map(x => x.RetweetCount).Name("retweet_count");
            Map(x => x.LikeCount).Name("like_count");
            Map(x => x.CreatedAt).Name("created_at");
            Map(x => x.Mentions).Convert(x => x.Row.GetField(15).Split("|").Distinct().ToList()).Name("mentions");
            Map(x => x.Hashtags).Convert(x => x.Row.GetField(16).Split("|").Distinct().ToList()).Name("hashtags");
        }
    }
}
