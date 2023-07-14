using CsvHelper.Configuration;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Accounts.Models
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(x => x.Id).Name("id");
            Map(x => x.Username).Name("screen_name");
            Map(x => x.Location).Name("location");
            Map(x => x.Name).Name("name");
            Map(x => x.Verified).Name("is_verified");
            Map(x => x.FollowersCount).Name("followers_count");
            Map(x => x.FollowingCount).Name("followees_count");
            Map(x => x.TweetCount).Name("tweets_count");
            Map(x => x.ListedCount).Name("listed_count");
            Map(x => x.ListedCount).Name("likes_count");
            Map(x => x.CreatedAt).Name("registration_date");
        }
    }
}
