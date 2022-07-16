using hospins.Repository.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hospins.Infrastructure
{
    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(CurrentContext.UserDetail == null)
            {
                filterContext.Result = (new RedirectController().RedirectToLogin(filterContext));
            }
            else if (CurrentContext.UserDetail.Username == null)
            {
                filterContext.Result = (new RedirectController().RedirectToLogin(filterContext));
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class RedirectController : Controller
    {
        public IActionResult RedirectToLogin(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Redirect(filterContext.HttpContext.Request.Path);
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
