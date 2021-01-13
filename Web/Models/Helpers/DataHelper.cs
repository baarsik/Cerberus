using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.Models.Helpers
{
    public class DataHelper
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataHelper(ApplicationContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Returns the number of unread notifications
        /// </summary>
        /// <param name="user">ApplicationUser entity</param>
        /// <returns>Number of unread notifications</returns>
        public async Task<int> GetNumberOfUnreadNotificationsAsync(ClaimsPrincipal user)
        {
            var applicationUser = await _userManager.GetUserAsync(user);
            return await _db.ApplicationUserNotifications.CountAsync(c => c.User == applicationUser && !c.IsRead);
        }
    }
}