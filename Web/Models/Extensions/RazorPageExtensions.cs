using Microsoft.AspNetCore.Mvc.Razor;

namespace Web.Models.Extensions
{
    public static class RazorPageExtensions
    {
        public static bool IsInDebugMode(this IRazorPage helper)
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif
        }
    }
}