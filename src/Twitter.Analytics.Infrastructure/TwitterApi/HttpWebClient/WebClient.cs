using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.Analytics.Infrastructure.Serialization;

namespace Twitter.Analytics.Infrastructure.TwitterApi.HttpWebClient
{
    public static class WebClient
    {
        public static async Task<TResult> ReadAsJsonAsync<TResult>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(json, new JsonSerializerOptions().SnnakeCase());
        }
    }
}
