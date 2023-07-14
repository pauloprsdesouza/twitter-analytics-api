using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Twitter.Analytics.Domain.Annotations.Models
{
    public class AnnotationMap : ClassMap<AnnotationModel>
    {
        public AnnotationMap()
        {
            Map(x => x.IdTweet).Name("id_tweet");
            Map(x => x.Domain).Name("domain");
            Map(x => x.Entity).Name("entity");

        }
    }
}
