using System.Collections.Generic;
using System.Linq;
using DataContext;
using DataContext.Models;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Models.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static ICollection<Language> GetUserOrDefaultLanguages(this ApplicationUser user, ApplicationContext dbContext, IConfiguration configuration)
        {
            if (user != null)
            {
                return user.UserLanguages?.OrderBy(c => c.Priority).Select(c => c.Language).ToList() ??
                                    dbContext.UserLanguages
                                        .Where(c => c.User == user)
                                        .OrderBy(c => c.Priority)
                                        .Select(c => c.Language)
                                        .ToList();
            }

            var defaultLanguageCodes = configuration["DefaultLanguages"].Split(",").ToList();
            return dbContext.Languages
                .Where(c => defaultLanguageCodes.Contains(c.Code))
                .OrderBy(c => defaultLanguageCodes.IndexOf(c.Code))
                .ToList();
        }
    }
}