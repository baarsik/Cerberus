﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cerberus.Models;

namespace Cerberus.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(WebNovelController.Index), "WebNovel");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
