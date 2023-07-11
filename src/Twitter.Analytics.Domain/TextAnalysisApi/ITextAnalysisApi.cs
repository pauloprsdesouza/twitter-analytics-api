using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.TextAnalysisApi.Models;

namespace Twitter.Analytics.Domain.TextAnalysisApi
{
    public interface ITextAnalysisApi
    {
        Task<TextResponseModel> GetTextAnalysis(string text);
    }
}
