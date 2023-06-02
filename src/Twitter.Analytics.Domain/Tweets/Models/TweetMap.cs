using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Twitter.Analytics.Domain.Tweets.Entities;

namespace Twitter.Analytics.Domain.Tweets.Models
{
    public class TweetMap : ClassMap<Tweet>
    {
        public TweetMap()
        {
            Map(x => x.Id).Name("id");
        }
    }
}
