using System.Collections.Generic;
using System.Linq;
using DataContext;
using DataContext.Models;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Models.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static ICollection<Language> GetUserLanguages(this ApplicationUser user, ApplicationContext dbContext)
        {
            if (user == null)
                return new List<Language>();
            
            return user.UserLanguages?.OrderBy(c => c.Priority).Select(c => c.Language).ToList() ??
                   dbContext.UserLanguages
                       .Where(c => c.User == user)
                       .OrderBy(c => c.Priority)
                       .Select(c => c.Language)
                       .ToList();

        }
        
        public static ICollection<Language> GetUserOrDefaultLanguages(this ApplicationUser user, ApplicationContext dbContext, IConfiguration configuration)
        {
            var userLanguages = user.GetUserLanguages(dbContext);
            if (userLanguages.Any())
                return userLanguages;

            var defaultLanguageCodes = configuration["DefaultLanguages"].Split(",").ToList();
            return dbContext.Languages
                .Where(c => defaultLanguageCodes.Contains(c.Code))
                .OrderBy(c => defaultLanguageCodes.IndexOf(c.Code))
                .ToList();
        }
    }
}