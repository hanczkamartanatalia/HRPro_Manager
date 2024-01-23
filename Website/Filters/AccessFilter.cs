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

        private void ControlAccess(ActionExecutingContext context)
        {
            
            string path = context.HttpContext.Request.Path;
            
            
            string? roleName = context.HttpContext.Session.GetString("R_Name");
            try
            {
                bool access = Service.AccountService.AccessService.HasAccess(path, roleName);
                if (!access)
                {
                    string? lastPath = context.HttpContext.Session.GetString("LastPath");
                    context.HttpContext.Session.SetString("Error",$"You have no access. {roleName} has no access to {path}.");
                    if (string.IsNullOrEmpty(lastPath) || lastPath == "/")
                    {
                        context.Result = new RedirectToActionResult("Index", "Home", null);

                    }
                    else
                    {
                        context.Result = new ContentResult
                        {
                            Content = "<script>history.go(-1);</script>",
                            ContentType = "text/html"
                        };
                    }
                }
                else
                {
                    context.HttpContext.Session.Remove("LastPath");
                    context.HttpContext.Session.SetString("LastPath", path);
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
