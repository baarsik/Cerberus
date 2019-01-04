﻿using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class PageController : BaseController
    {
        public IActionResult Rules()
        {
            return View();
        }

        [Route("error/404.html")]
        public new IActionResult NotFound()
        {
            return View();
        }
    }
}