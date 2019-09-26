using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;

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

        protected IActionResult RedirectToNotFound()
        {
            Response.StatusCode = 404;
            return RedirectToLocal("/error/404.html");
        }

        protected async Task ValidateReCaptchaAsync(IRecaptchaService recaptchaService)
        {
            var recaptchaValidationResult = await recaptchaService.Validate(Request);
            if (!recaptchaValidationResult.success)
            {
                ModelState.AddModelError("", "There was an error validating reCaptcha");
            }
        }
    }
}