using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analytics.Domain.Annotations.Models
{
    public class AnnotationModel
    {
        public string IdTweet { get; set; }
        public string Domain { get; set; }
        public string Entity { get; set; }
    }
}
