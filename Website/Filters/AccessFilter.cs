using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Website.Filters
{
    public class AccessFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string path = context.HttpContext.Request.Path;
            string? roleName = context.HttpContext.Session.GetString("R_Name");
            try
            {
                bool access = Service.AccountService.AccessService.HasAccess(path, roleName);
                if (!access)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
            catch
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ControlAccess(context);
        }

        private void ControlAccess(ActionExecutedContext context)
        {
            string path = context.HttpContext.Request.Path;
            string? roleName = context.HttpContext.Session.GetString("R_Name");
            try
            {
                bool access = Service.AccountService.AccessService.HasAccess(path, roleName);
                if (!access)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
            catch
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
