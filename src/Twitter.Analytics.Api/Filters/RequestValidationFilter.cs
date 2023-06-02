using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Twitter.Analytics.Api.Filters
{
    public class RequestValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .ToDictionary(entry => entry.Key, entry => entry.Value.Errors
                        .Select(modelError => modelError.ErrorMessage)
                        .ToList());

                context.Result = new JsonResult(new { errors })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
