using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Website.Filters
{
    public class AccessFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ControlAccess(context);
            //TODO przy cofaniu do home nie wyświetla tam komunikatu tylko w kolejno klikniętej stronie
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
                    context.HttpContext.Session.SetString("Error","You have no access.");
                    context.Result = new ContentResult
                    {
                        Content = "<script>history.go(-1)</script>",
                        ContentType = "text/html"
                    };
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
