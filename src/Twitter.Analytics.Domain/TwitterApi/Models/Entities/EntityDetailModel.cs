using System.Collections.Generic;

namespace Twitter.Analytics.Domain.TwitterApi.Models.Entities
{
    public class EntityDetailModel
    {
        public List<MentionModel> Mentions { get; set; }
        public List<UrlModel> Urls { get; set; }
        public List<HashtagModel> Hashtags { get; set; }

        public EntityDetailModel()
        {
            Mentions = new();
            Urls = new();
            Hashtags = new();
        }
    }
}
