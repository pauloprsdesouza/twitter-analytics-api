using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Twitter.Analytics.Domain.Mentions.Models
{
    public class MentionMap : ClassMap<MentionModel>
    {
        public MentionMap()
        {
            Map(x => x.IdTweet).Name("id_tweet");
            Map(x => x.IdUser).Name("id_user_mentioned");
        }
    }
}
