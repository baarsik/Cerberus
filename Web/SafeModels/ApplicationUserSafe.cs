using System;
using DataContext.Models;

namespace Web.SafeModels
{
    public class ApplicationUserSafe
    {
        public string DisplayName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Avatar { get; set; }
            
        public static ApplicationUserSafe Convert(ApplicationUser user) =>
            new()
            {
                DisplayName = user.DisplayName,
                RegisterDate = user.RegisterDate,
                Avatar = user.Avatar
            };
    }
}