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
    }
}