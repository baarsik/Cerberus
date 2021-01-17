using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Extensions;
using Web.Models.Helpers;
using Web.Models.Services;
using Web.Models.ViewModels;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Web.Controllers.Services
{
    public sealed class AuthService : BaseService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            IDbContextFactory<ApplicationContext> dbContextFactory, IWebHostEnvironment webHostEnvironment)
        : base(dbContextFactory, userManager, configuration)
        {
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _context = DbContextFactory.CreateDbContext();
        }

        /// <summary>
        /// Checks whether Display Name is already in use
        /// </summary>
        public async Task<bool> IsDisplayNameInUseAsync(string displayName)
        {
            return await UserManager.Users.AnyAsync(c => c.DisplayName == displayName);
        }
        
        /// <summary>
        /// Checks whether E-Mail is already in use
        /// </summary>
        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await UserManager.Users.AnyAsync(c => c.Email == email);
        }

        /// <summary>
        /// Returns ApplicationUser record based on email and password
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByCredentialsAsync(string email, string password)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(c => c.Email == email);
            
            return user != null && await UserManager.CheckPasswordAsync(user, password)
                ? user
                : null;
        }
        
        /// <summary>
        /// Returns ApplicationUser record based on display name
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByDisplayNameAsync(string displayName)
        {
            return await UserManager.Users
                .FirstOrDefaultAsync(c => c.DisplayName == displayName);
        }
        
        /// <summary>
        /// Returns ApplicationUser record based on display name and password
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByDisplayNameAsync(string displayName, string password)
        {
            var user = await GetUserByDisplayNameAsync(displayName);
            return user != null && await UserManager.CheckPasswordAsync(user, password)
                ? user
                : null;
        }
        
        public async Task<LoginResult> LoginAsync(string email, string password, bool isPersistent = false)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return new LoginResult(LoginStatus.InvalidCredentials);

            if (user.LockoutEnd?.DateTime >= DateTime.Now)
                return new LoginResult(LoginStatus.AccountLocked);
            
            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, true);
            return new LoginResult(result.Succeeded
                ? LoginStatus.Success
                : LoginStatus.InvalidCredentials);
        }
        
        public async Task<RegisterResult> RegisterAsync(string email, string displayName, string password, IList<Guid> languageIds)
        {
            displayName = displayName.RemoveHTML().FixSpacing();
            
            var user = new ApplicationUser
            {
                DisplayName = displayName,
                UserName = email,
                Email = email,
                ApiKey = StringHelpers.GenerateRandomString(64)
            };

            if (await IsDisplayNameInUseAsync(displayName))
                return new RegisterResult(RegisterStatus.DisplayNameInUse);
            
            if (await IsEmailInUseAsync(email))
                return new RegisterResult(RegisterStatus.EmailInUse);

            var languages = (await GetLanguagesAsync())
                .Where(c => languageIds.Contains(c.Id))
                .OrderBy(c => languageIds.IndexOf(c.Id))
                .ToList();
            if (!languages.Any(c => languageIds.Contains(c.Id)))
                return new RegisterResult(RegisterStatus.LanguageNotExists);
            
            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new RegisterResult(RegisterStatus.Failure);

            await UserManager.AddClaimAsync(user, new Claim("DisplayName", user.DisplayName));
            await UserManager.AddToRoleAsync(user, Constants.Roles.User);
            await _signInManager.SignInAsync(user, false);

            var priority = 0;
            var userLanguages = languages.Select(c => new ApplicationUserLanguage
            {
                Id = Guid.NewGuid(),
                Language = c,
                Priority = priority++,
                User = user
            });
            
            _context.UserLanguages.AddRange(userLanguages);
            await _context.SaveChangesAsync();
            
            return new RegisterResult(RegisterStatus.Success);
        }

        public async Task<EditProfileViewModel> GetEditProfileViewModel(ApplicationUser user)
        {
            var model = new EditProfileViewModel
            {
                User = user,
                Languages = await GetLanguagesForEditProfileViewModel(user),
                SelectedLanguages = await _context.UserLanguages
                    .Where(c => c.User == user)
                    .Select(c => c.Language.Id)
                    .ToListAsync()
            };

            return model;
        }
        
        public async Task UpdateProfile(EditProfileViewModel model)
        {
            var currentLanguages = await _context.UserLanguages
                .Where(c => c.User == model.User)
                .ToListAsync();
            _context.UserLanguages.RemoveRange(currentLanguages);

            if (model.SelectedLanguages != null)
            {
                var languages = (await GetLanguagesAsync())
                    .Where(c => model.SelectedLanguages.Contains(c.Id))
                    .OrderBy(c => model.SelectedLanguages.IndexOf(c.Id))
                    .ToList();
                
                var priority = 0;
                var userLanguages = languages.Select(c => new ApplicationUserLanguage
                {
                    Id = Guid.NewGuid(),
                    Language = c,
                    Priority = priority++,
                    User = model.User
                });
            
                _context.UserLanguages.AddRange(userLanguages);
            }

            if (!string.IsNullOrEmpty(model.Avatar?.FileName) && model.Avatar.ContentType.StartsWith("image"))
            {
                if (!string.IsNullOrEmpty(model.User.Avatar) &&
                    File.Exists($"{_webHostEnvironment.WebRootPath}/images/avatars/{model.User.Avatar}.png"))
                {
                    File.Delete($"{_webHostEnvironment.WebRootPath}/images/avatars/{model.User.Avatar}.png");
                }
                model.User.Avatar = $"{model.User.Id}_{Guid.NewGuid()}"; // avoid caching
                await UploadAvatarAsync(model.Avatar, model.User.Avatar);
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Generate JSON Web Token based on user, update last token IP
        /// </summary>
        /// <returns>Token</returns>
        public async Task<GenerateJwtResult> GenerateJwtAsync(string email, string apiKey, string ip)
        {
            var user = await UserManager.Users
                .FirstOrDefaultAsync(c => c.Email == email && c.ApiKey == apiKey);
            
            if (user == null)
                return new GenerateJwtResult(false);

            var tokenId = Guid.NewGuid();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(Configuration["JwtExpireHours"]));

            var jwt = new JwtSecurityToken(
                issuer: Configuration["JwtIssuer"],
                audience: Configuration["JwtIssuer"],
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString()),
                    new Claim(ClaimTypes.SerialNumber, user.ApiKey)
                },
                expires: expires,
                signingCredentials: credentials
            );
            
            // Update user token binding
            var userFromDb = await _context.Users.FirstAsync(c => c.Id == user.Id);
            userFromDb.ApiBindedIp = ip;
            userFromDb.LastApiTokenId = tokenId;
            _context.Entry(userFromDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new GenerateJwtResult(user, token);
        }

        /// <summary>
        /// Validates whether user is eligible for API access
        /// </summary>
        /// <returns>Is valid user</returns>
        public async Task<bool> IsApiBindedIpValidAsync(ClaimsPrincipal user, HttpContext httpContext)
        {
            if (!Guid.TryParse(user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value, out var tokenId))
                return false;
            
            var email = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var apiKey = user.FindFirst(ClaimTypes.SerialNumber)?.Value;
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            return await IsApiBindedIpValidAsync(tokenId, email, apiKey, ip);
        }

        /// <summary>
        /// Validates whether user is eligible for API access
        /// </summary>
        /// <param name="tokenId">JWT id (claim JwtRegisteredClaimNames.Jti)</param>
        /// <param name="email">Current user E-Mail</param>
        /// <param name="apiKey">Current user API key</param>
        /// <param name="ip">Current user IP</param>
        /// <returns>Is valid user</returns>
        public async Task<bool> IsApiBindedIpValidAsync(Guid tokenId, string email, string apiKey, string ip)
        {
            return await _context.Users.AnyAsync(c => c.Email == email && c.ApiKey == apiKey && c.ApiBindedIp == ip && c.LastApiTokenId == tokenId);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="user">User to change password</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            var result = await UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        /// <summary>
        /// Generates new API key
        /// </summary>
        /// <param name="user">User to change API key</param>
        public async Task RegenerateApiKey(ApplicationUser user)
        {
            user.ApiKey = StringHelpers.GenerateRandomString(64);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Language>> GetLanguagesForEditProfileViewModel(ApplicationUser user)
        {
            if (user == null)
                return await GetLanguagesAsync();

            var languages = user.GetUserLanguages(_context);
            foreach (var language in await _context.Languages.Where(dbLang => languages.All(userLang => !Equals(userLang, dbLang))).ToListAsync())
            {
                languages.Add(language);
            }

            return languages;
        }
        
        private async Task UploadAvatarAsync(IFormFile formFile, string fileName)
        {
            if (!formFile.ContentType.StartsWith("image"))
                return;
            
            using var image = await Image.LoadAsync(formFile.OpenReadStream());
            var shortestSize = image.Width > image.Height ? image.Height : image.Width;
            var cropRectangle = new Rectangle((image.Width - shortestSize) / 2, (image.Height - shortestSize) / 2, shortestSize, shortestSize);
            image.Mutate(x => x
                .Crop(cropRectangle)
                .Resize(Constants.Profile.AvatarSize, Constants.Profile.AvatarSize));
            Directory.CreateDirectory($"{_webHostEnvironment.WebRootPath}/images/avatars/");
            await image.SaveAsPngAsync($"{_webHostEnvironment.WebRootPath}/images/avatars/{fileName}.png");
        }
    }
}
