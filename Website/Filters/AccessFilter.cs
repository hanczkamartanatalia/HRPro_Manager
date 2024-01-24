using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website.Filters
{
    public class AccessFilter : IActionFilter
    {
           
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ControlAccess(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static void ControlAccess(ActionExecutingContext context)
        {
            
            string path = context.HttpContext.Request.Path;
            string roleName = context.HttpContext.Session.GetString("R_Name") ?? string.Empty;
            try
            {
                bool access = Service.AccountService.AccessService.HasAccess(path, roleName);
                if (!access)
                {
                    string controllerName = context.HttpContext.Session.GetString("controllerName") ?? "Home";
                    string actionName = context.HttpContext.Session.GetString("actionName") ?? "Index";

                    context.HttpContext.Session.SetString("Error","You have no access.");
                    context.Result = new RedirectToActionResult(actionName, controllerName, null);

                }
                else
                {
                    string controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Home";
                    string actionName = context.RouteData.Values["action"]?.ToString() ?? "Index";
                    context.HttpContext.Session.Remove("controllerName");
                    context.HttpContext.Session.Remove("actionName");
                    context.HttpContext.Session.SetString("controllerName", controllerName);
                    context.HttpContext.Session.SetString("actionName", actionName);
                }
            }
            catch
            {
                context.HttpContext.Session.SetString("Error", "Page not found");
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
