using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Api.Configuration;
using Twitter.Analytics.Api.DependencyInjection;
using Twitter.Analytics.Api.Filters;
using Twitter.Analytics.Infrastructure.Jobs;
using Twitter.Analytics.Infrastructure.Serialization;

namespace  Twitter.Analytics.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            Configure(builder.Build());
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
                options.Filters.Add(typeof(RequestValidationFilter));
                options.Filters.Add(typeof(NotificationFilter));
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Default());

            services.AddSwaggerDocumentation();
            services.AddTwitterApi(configuration.GetSection("TwitterCredential"));
            services.AddServices();
            services.AddNotifications();
            services.AddMapperProfiles();
            services.AddRepositories();
            services.AddDynamoDBDependency();
            services.AddHostedService<UpdateTweetsJob>();
        }

        public static void Configure(WebApplication app)
        {
            app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
