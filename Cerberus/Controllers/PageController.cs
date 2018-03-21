using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class PageController : BaseController
    {
        public IActionResult Rules(string name)
        {
            return View();
        }
    }
}