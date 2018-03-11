using System.Security.Claims;

namespace Cerberus.Models.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("admin");
        }
    }
}