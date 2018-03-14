using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    [Authorize(Roles = "admin")]
    public class Settings : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}