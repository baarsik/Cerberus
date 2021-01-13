using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models.Extensions
{
    public static class ViewContextExtensions
    {
        public static bool IsActive(this ViewContext viewContext, string controller)
        {
            var controllerValue = (string) viewContext.RouteData.Values["Controller"];
            return string.Compare(controllerValue, controller, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        
        public static bool IsActive(this ViewContext viewContext, string action, string controller)
        {
            if (!viewContext.IsActive(controller))
                return false;
            
            var actionValue = (string) viewContext.RouteData.Values["Action"];
            return string.Compare(actionValue, action, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        
        public static bool IsActive(this ViewContext viewContext, IEnumerable<string> actions, string controller)
        {
            if (!viewContext.IsActive(controller))
                return false;
            
            var actionValue = (string) viewContext.RouteData.Values["Action"];
            return actions.Any(action => string.Compare(actionValue, action, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        
        public static string FillClassIfActive(this ViewContext viewContext, string controller)
        {
            return viewContext.IsActive(controller)
                ? "active"
                : string.Empty;
        }
        
        public static string FillClassIfActive(this ViewContext viewContext, string action, string controller)
        {
            return viewContext.IsActive(action, controller)
                ? "active"
                : string.Empty;
        }
        
        public static string FillClassIfActive(this ViewContext viewContext, IEnumerable<string> actions, string controller)
        {
            return viewContext.IsActive(actions, controller)
                ? "active"
                : string.Empty;
        }
    }
}