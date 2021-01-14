using System.Linq;
using System.Security.Claims;
using AngleSharp.Text;

namespace Web.Models.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Checks whether user is in role "admin"
        /// </summary>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(Constants.Roles.Admin);
        }
        
        public static bool HasModerateAccess(this ClaimsPrincipal user)
        {
            return Constants.Permissions.Moderate.SplitCommas().Any(user.IsInRole);
        }
        
        public static bool HasWebNovelEditorAccess(this ClaimsPrincipal user)
        {
            return Constants.Permissions.WebNovelEdit.SplitCommas().Any(user.IsInRole);
        }
        
        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return user?.Identity?.IsAuthenticated == true
                ? user.Claims.Where(c => c.Type == "DisplayName").Select(c => c.Value).FirstOrDefault()
                : string.Empty;
        }
    }
}