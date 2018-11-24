using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cerberus.Models.Extensions
{
    public static class ViewContextExtensions
    {
        public static bool IsActive(this ViewContext viewContext, string action, string controller)
        {
            var controllerValue = (string) viewContext.RouteData.Values["Controller"];
            var actionValue = (string) viewContext.RouteData.Values["Action"];
            return string.Compare(controllerValue, controller, StringComparison.InvariantCultureIgnoreCase) == 0
                   && string.Compare(actionValue, action, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        
        public static string FillClassIfActive(this ViewContext viewContext, string action, string controller)
        {
            return viewContext.IsActive(action, controller)
                ? "active"
                : "";
        }
    }
}