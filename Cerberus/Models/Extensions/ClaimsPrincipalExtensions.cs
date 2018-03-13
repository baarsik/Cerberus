using System.Linq;
using System.Security.Claims;

namespace Cerberus.Models.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Checks whether user is in role "admin"
        /// </summary>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("admin");
        }
        
        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated
                ? user.Claims.FirstOrDefault(c => c.Type == "DisplayName")?.Value
                : "";
        }
    }
}