using System.Text.Json;
using System.Text.Json.Serialization;

namespace Twitter.Analytics.Infrastructure.Serialization
{
    public static class ApiJsonSerializerOptions
    {
        public static JsonSerializerOptions Default(this JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new UlidJsonConverter());
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            return options;
        }

         public static JsonSerializerOptions SnnakeCase(this JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new UlidJsonConverter());
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            return options;
        }
    }
}
