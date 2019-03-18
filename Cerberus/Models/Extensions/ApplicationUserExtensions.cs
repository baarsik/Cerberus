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
            if (user?.UserLanguages?.Any() == true)
                return user.UserLanguages.Select(c => c.Language).ToList();

            var defaultLanguageCodes = configuration["DefaultLanguages"].Split(",").ToList();
            return dbContext.Languages
                .Where(c => defaultLanguageCodes.Contains(c.Code))
                .OrderBy(c => defaultLanguageCodes.IndexOf(c.Code))
                .ToList();
        }
    }
}