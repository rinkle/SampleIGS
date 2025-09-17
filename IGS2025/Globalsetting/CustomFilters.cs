using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Globalsetting
{
    public sealed class RemoveCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            response.Headers["Pragma"] = "no-cache";
            response.Headers["Expires"] = "0";
            base.OnResultExecuting(filterContext);
        }
    }

    /// <summary>
    /// Restricts action to be called only via AJAX requests.
    /// </summary>
    public sealed class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (!isAjax)
            {
                context.Result = new BadRequestResult(); // ❌ block non-AJAX calls
            }
            base.OnActionExecuting(context);
        }
    }
}
