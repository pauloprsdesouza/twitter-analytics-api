using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets.Models
{
    public class TweetResponseMap: ClassMap<Tweet>
    {
        public TweetResponseMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.AuthorId).Name("authorId");
            Map(x => x.Text).Name("text");
            Map(x => x.RetweetCount).Name("retweetCount");
            Map(x => x.ReplyCount).Name("replyCount");
            Map(x => x.LikeCount).Name("likeCount");
            Map(x => x.QuoteCount).Name("quoteCount");
            Map(x => x.ImpressionCount).Name("impressionCount");
            Map(x => x.ContextScore).Name("contextScore");
            Map(x => x.DiversityScore).Name("diversityScore");
            Map(x => x.SentimentScore).Name("sentimentScore");
            Map(x => x.Tokens).Name("tokens");
            Map(x => x.Urls).Name("urls");
            Map(x => x.Mentions).Name("mentions");
            Map(x => x.Hashtags).Name("hashtags");
            Map(x => x.Domains).Name("domains");
            Map(x => x.Entities).Name("entities");
            Map(x => x.EngagementScore).Name("engagementScore");
            Map(x => x.RecencyScore).Name("recencyScore");
            Map(x => x.SocialCapitalScore).Name("socialCapitalScore");
            Map(x => x.CreatedAt).Name("created_at");
        }
    }
}
