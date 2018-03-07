using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                ? (IActionResult)Redirect(returnUrl)
                : RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}