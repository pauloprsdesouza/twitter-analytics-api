using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Aws.Tools.Message.SNS;
using Aws.Tools.Message.SQS;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Analytics.Domain.Notifications;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class NotificationDependency
    {
        public static void AddNotifications(this IServiceCollection services)
        {
            services.AddScoped<INotificationContext, NotificationContext>();
        }
    }
}
