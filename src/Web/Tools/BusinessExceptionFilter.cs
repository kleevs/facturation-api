using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Tools
{
    public class BusinessExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception as FacturationApi.Tools.Error;
            if (exception != null)
            {
                var result = new ObjectResult(exception.Content);
                context.ExceptionHandled = true;
                context.Result = result;
                result.StatusCode = exception.StatusCode ?? 400;
            }
        }
    }
}