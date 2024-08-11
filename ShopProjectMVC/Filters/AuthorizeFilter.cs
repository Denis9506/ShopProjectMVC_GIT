using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopProjectMVC.Filters
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        private readonly int _requiredRole = 0;

        public AuthorizeFilter(int requiredRole)
        {
            _requiredRole = requiredRole;
        }
        public AuthorizeFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            if (session == null || session.GetString("userId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
                return;
            }

            if (_requiredRole!=0)
            {
                var userRole = session.GetInt32("role");

                if (userRole!=_requiredRole)
                {
                    context.Result = new RedirectToActionResult("Login", "User", null);
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
