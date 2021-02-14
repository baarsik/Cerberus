using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult RedirectToLocal(string returnUrl = "/")
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return Redirect(returnUrl);
        }

        protected IActionResult RedirectToNotFound()
        {
            Response.StatusCode = 404;
            return RedirectToLocal("/error/404.html");
        }
    }
}