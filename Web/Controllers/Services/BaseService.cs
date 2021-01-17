﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Extensions;
using Web.Models.Helpers;
using Web.Models.Services;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Web.Controllers.Services
{
    public abstract class BaseService
    {
        protected readonly IDbContextFactory<ApplicationContext> DbContextFactory;
        protected readonly UserManager<ApplicationUser> UserManager;
        protected readonly IConfiguration Configuration;
        
        private readonly ApplicationContext _db;

        public BaseService(IDbContextFactory<ApplicationContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            DbContextFactory = dbContextFactory;
            UserManager = userManager;
            Configuration = configuration;
            _db = dbContextFactory.CreateDbContext();
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
                .GetUserOrDefaultLanguages(_db, Configuration)
                .Select(c => c.Code)
                .FirstOrDefault();
        }
        
        /// <summary>
        /// Returns all languages available on server
        /// </summary>
        /// <returns>List of Language</returns>
        public async Task<IList<Language>> GetLanguagesAsync()
        {
            return await _db.Languages.ToListAsync();
        }
        
        /// <summary>
        /// Returns all languages available on server with the exclusion of listed languages
        /// </summary>
        /// <param name="excludedLanguages">List of Language to exclude</param>
        /// <returns>List of Language</returns>
        public async Task<IEnumerable<Language>> GetLanguagesAsync(IEnumerable<Language> excludedLanguages)
        {
            return await _db.Languages.Where(c => !excludedLanguages.Contains(c)).ToListAsync();
        }

        /// <summary>
        /// Adds notification to the specified user
        /// </summary>
        /// <param name="user">ApplicationUser entity</param>
        /// <param name="body">Notification body (HTML enabled)</param>
        public async Task AddNotificationAsync(ApplicationUser user, string body)
        {
            _db.Add(new ApplicationUserNotifications
            {
                Id = Guid.NewGuid(),
                Body = body,
                NotificationDate = DateTime.Now,
                User = user
            });
            
            await _db.SaveChangesAsync();
        }
    }
}
