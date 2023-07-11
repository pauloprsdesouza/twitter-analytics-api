using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Twitter.Analytics.Domain.TextAnalysisApi;
using Twitter.Analytics.Domain.TextAnalysisApi.Models;

namespace Twitter.Analytics.Infrastructure.TextAnalaysisApi
{
    public class TextAnalysisApi : ITextAnalysisApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<TextAnalysisApi> _logger;

        public TextAnalysisApi(HttpClient client, ILogger<TextAnalysisApi> logger)
        {
            _client = client;
            _logger = logger;
        }


        public async Task<TextResponseModel> GetTextAnalysis(string text)
        {
            var requestBody = new StringContent(JsonSerializer.Serialize(text), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("process_text", requestBody);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response:");
                Console.WriteLine(responseContent);

                return null;
            }

            return JsonSerializer.Deserialize<TextResponseModel>(await response.Content.ReadAsStringAsync());
        }
    }
}
