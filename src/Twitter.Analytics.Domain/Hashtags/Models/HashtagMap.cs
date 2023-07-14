using CsvHelper.Configuration;

namespace Twitter.Analytics.Domain.Hashtags.Models
{
    public class HashtagMap : ClassMap<HashtagModel>
    {
        public HashtagMap()
        {
            Map(x => x.IdTweet).Name("id_tweet");
            Map(x => x.Name).Name("name");
        }
    }
}
