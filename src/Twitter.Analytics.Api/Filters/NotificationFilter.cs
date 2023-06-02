using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Twitter.Analytics.Domain.Notifications;
using Twitter.Analytics.Infrastructure.Serialization;
using Twitter.Analytics.Contracts;

namespace Twitter.Analytics.Api.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly INotificationContext _notification;

        public NotificationFilter(INotificationContext notification)
        {
            _notification = notification;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notification.AreThereValidationErrors())
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonSerializer.Serialize(new ResponseError(_notification.GetValidationErrors()), new JsonSerializerOptions().Default());
                await context.HttpContext.Response.WriteAsync(notifications);
                return;
            }

            if (_notification.AreThereNotFoundErros())
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonSerializer.Serialize(new ResponseError(_notification.GetNotFoundErrors()), new JsonSerializerOptions().Default());
                await context.HttpContext.Response.WriteAsync(notifications);
                return;
            }

            if (_notification.AreThereForbiddenErros())
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonSerializer.Serialize(new ResponseError(_notification.GetForbiddenErrors()), new JsonSerializerOptions().Default());
                await context.HttpContext.Response.WriteAsync(notifications);
                return;
            }

            await next();
        }
    }
}
