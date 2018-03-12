﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Models;
using Cerberus.Models.Services;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cerberus.Controllers.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Checks whether Display Name is already in use
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public async Task<bool> IsDisplayNameInUseAsync(string displayName)
        {
            return await _userManager.Users.AnyAsync(c => string.Compare(c.DisplayName, displayName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        
        /// <summary>
        /// Checks whether E-Mail is already in use
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await _userManager.Users.AnyAsync(c => string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        /// <summary>
        /// Returns ApplicationUser record based on email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>ApplicationUser on success, null on failure</returns>
        public async Task<ApplicationUser> GetUserByCredentials(string email, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
            
            return user != null && await _userManager.CheckPasswordAsync(user, password)
                ? user
                : null;
        }

        public async Task<LoginResult> JwtLoginAsync(string email, string password, bool isPersistent = false)
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
            var user = new ApplicationUser
            {
                DisplayName = displayName,
                UserName = email,
                Email = email
            };

            if (await IsDisplayNameInUseAsync(displayName))
                return new RegisterResult(RegisterStatus.DisplayNameInUse);
            
            if (await IsEmailInUseAsync(email))
                return new RegisterResult(RegisterStatus.EmailInUse);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new RegisterResult(RegisterStatus.Failure);

            await _userManager.AddToRoleAsync(user, "user");
            await _signInManager.SignInAsync(user, false);
            return new RegisterResult(RegisterStatus.Success);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public string GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtExpireHours"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtIssuer"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
