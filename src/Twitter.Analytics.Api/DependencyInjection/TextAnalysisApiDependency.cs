using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Domain.TextAnalysisApi;
using Twitter.Analytics.Infrastructure.TextAnalaysisApi;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class TextAnalysisApiDependency
    {
        public static void AddTextAnalysisApi(this IServiceCollection services)
        {
            services.AddHttpClient<ITextAnalysisApi, TextAnalysisApi>("TextAnalysis", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }
    }
}
