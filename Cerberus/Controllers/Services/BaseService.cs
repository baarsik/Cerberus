using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Models;
using Cerberus.Models.Extensions;
using Cerberus.Models.Helpers;
using Cerberus.Models.Services;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cerberus.Controllers.Services
{
    public abstract class BaseService
    {
        protected readonly UserManager<ApplicationUser> UserManager;
        protected readonly IConfiguration Configuration;
        protected readonly ApplicationContext Db;

        public BaseService(ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            UserManager = userManager;
            Configuration = configuration;
            Db = context;
        }

        /// <summary>
        /// Returns ApplicationUser record based on ClaimsIdentity
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await UserManager.GetUserAsync(user);
        }

        /// <summary>
        /// Returns language code of user-preferred or default language for the user
        /// </summary>
        /// <param name="user">ApplicationUser entity</param>
        /// <returns>Language code (ex. "en")</returns>
        public string GetDefaultUserLanguageCode(ApplicationUser user)
        {
            return user
                .GetUserOrDefaultLanguages(Db, Configuration)
                .Select(c => c.Code)
                .FirstOrDefault();
        }
        
        /// <summary>
        /// Returns all languages available on server
        /// </summary>
        /// <returns>List of Language</returns>
        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await Db.Languages.ToListAsync();
        }
        
        /// <summary>
        /// Returns all languages available on server with the exclusion of listed languages
        /// </summary>
        /// <param name="excludedLanguages">List of Language to exclude</param>
        /// <returns>List of Language</returns>
        public async Task<IEnumerable<Language>> GetLanguagesAsync(IEnumerable<Language> excludedLanguages)
        {
            return await Db.Languages.Where(c => !excludedLanguages.Contains(c)).ToListAsync();
        }
    }
}
