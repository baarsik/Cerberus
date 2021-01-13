using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers.Services;
using DataContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Models.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static void UpdateCultureCookie(this ControllerBase controllerBase, BaseService baseService, ApplicationUser user)
        {
            var culture = baseService.GetDefaultUserLanguageCode(user);
            controllerBase.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions {Expires = DateTimeOffset.UtcNow.AddYears(5)});
        }
    }
}