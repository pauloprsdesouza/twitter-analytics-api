using CsvHelper.Configuration;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Accounts.Models
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Username).Name("username");
            Map(x => x.Location).Name("location");
            Map(x => x.Name).Name("name");
            Map(x => x.Verified).Name("verified");
            Map(x => x.FollowersCount).Name("followers_count");
            Map(x => x.FollowingCount).Name("following_count");
            Map(x => x.TweetCount).Name("tweet_count");
            Map(x => x.ListedCount).Name("listed_count");
            Map(x => x.CreatedAt).Name("created_at");
        }
    }
}
