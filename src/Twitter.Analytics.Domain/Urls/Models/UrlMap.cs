using CsvHelper.Configuration;

namespace Twitter.Analytics.Domain.Urls.Models
{
    public class UrlMap : ClassMap<UrlModel>
    {
        public UrlMap()
        {
            Map(x => x.IdTweet).Name("id_tweet");
            Map(x => x.Name).Name("name");
        }
    }
}
