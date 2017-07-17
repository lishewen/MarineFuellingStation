using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AxiosAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var c = context.Controller as ControllerBase;
            if (string.IsNullOrWhiteSpace(c.UserName))
            {
                c.UserName = c.Request.Headers["x-username"];
            }
            base.OnActionExecuting(context);
        }
    }
}
