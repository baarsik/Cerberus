using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public sealed class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _db;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = context;
        }

        /// <summary>
        /// Checks whether Display Name is already in use
        /// </summary>
        public async Task<bool> IsDisplayNameInUseAsync(string displayName)
        {
            return await _userManager.Users.AnyAsync(c => string.Compare(c.DisplayName, displayName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        
        /// <summary>
        /// Checks whether E-Mail is already in use
        /// </summary>
        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await _userManager.Users.AnyAsync(c => string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        /// <summary>
        /// Returns ApplicationUser record based on email and password
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByCredentialsAsync(string email, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
            
            return user != null && await _userManager.CheckPasswordAsync(user, password)
                ? user
                : null;
        }
        
        /// <summary>
        /// Returns ApplicationUser record based on display name
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByDisplayNameAsync(string displayName)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(c => string.Compare(c.DisplayName, displayName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        
        /// <summary>
        /// Returns ApplicationUser record based on display name and password
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByDisplayNameAsync(string displayName, string password)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(c => string.Compare(c.DisplayName, displayName, StringComparison.InvariantCultureIgnoreCase) == 0);
            
            return user != null && await _userManager.CheckPasswordAsync(user, password)
                ? user
                : null;
        }

        /// <summary>
        /// Returns ApplicationUser record based on ClaimsIdentity
        /// </summary>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByClaimsPrincipalAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
        
        public async Task<LoginResult> LoginAsync(string email, string password, bool isPersistent = false)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (user == null)
                return new LoginResult(LoginStatus.InvalidCredentials);

            if (user.LockoutEnd?.DateTime >= DateTime.Now)
                return new LoginResult(LoginStatus.AccountLocked);

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, true);
            return new LoginResult(result.Succeeded
                ? LoginStatus.Success
                : LoginStatus.InvalidCredentials);
        }
        
        public async Task<RegisterResult> RegisterAsync(string email, string displayName, string password)
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

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new RegisterResult(RegisterStatus.Failure);

            await _userManager.AddClaimAsync(user, new Claim("DisplayName", user.DisplayName));
            await _userManager.AddToRoleAsync(user, Constants.Roles.User);
            await _signInManager.SignInAsync(user, false);
            return new RegisterResult(RegisterStatus.Success);
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
            var user = await _userManager.Users
                .FirstOrDefaultAsync(c =>
                    string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0 
                    && c.ApiKey == apiKey);
            
            if (user == null)
                return new GenerateJwtResult(false);

            var tokenId = Guid.NewGuid();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtExpireHours"]));

            var jwt = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtIssuer"],
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
            var userFromDb = await _db.Users.FirstAsync(c => c.Id == user.Id);
            userFromDb.ApiBindedIp = ip;
            userFromDb.LastApiTokenId = tokenId;
            _db.Entry(userFromDb).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            
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
            var ip = httpContext.Connection.RemoteIpAddress.ToString();
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
            return await _db.Users.AnyAsync(c => c.Email == email && c.ApiKey == apiKey && c.ApiBindedIp == ip && c.LastApiTokenId == tokenId);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="user">User to change password</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        public async Task ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        /// <summary>
        /// Generates new API key
        /// </summary>
        /// <param name="user">User to change API key</param>
        public async Task RegenerateApiKey(ApplicationUser user)
        {
            user.ApiKey = StringHelpers.GenerateRandomString(64);
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
