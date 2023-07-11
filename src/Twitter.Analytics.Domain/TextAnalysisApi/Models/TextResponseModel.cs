using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Analytics.Domain.TextAnalysisApi.Models
{
    public class TextResponseModel
    {
        public decimal ContextScore { get; set; }
        public decimal DiversityScore { get; set; }
        public decimal SentimentSecore { get; set; }
        public List<string> Tokens { get; set; }
    }
}
