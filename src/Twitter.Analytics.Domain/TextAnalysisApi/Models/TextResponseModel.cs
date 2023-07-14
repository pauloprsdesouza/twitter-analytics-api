using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analytics.Domain.TextAnalysisApi.Models
{
    public class TextResponseModel
    {
        public double ContextScore { get; set; }
        public double DiversityScore { get; set; }
        public double SentimentScore { get; set; }
        public List<string> Tokens { get; set; }
    }
}
