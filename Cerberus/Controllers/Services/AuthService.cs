using System;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<bool> UserExistsAsync(string email, string login)
        {
            return await _userManager.Users.AnyAsync(c =>
                string.Compare(c.Email, email, StringComparison.InvariantCultureIgnoreCase) != 0 ||
                string.Compare(c.DisplayName, login, StringComparison.InvariantCultureIgnoreCase) != 0);
        }

        public async Task<JwtLoginResult> JwtLoginAsync(string login, string password)
        {
            return await JwtLoginAsync(login, password, false);
        }

        public async Task<JwtLoginResult> JwtLoginAsync(string login, string password, bool isPersistent)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(r => r.UserName == login);
            if (user == null)
                return new JwtLoginResult(LoginStatus.InvalidCredentials);

            if (user.LockoutEnd?.DateTime >= DateTime.Now)
                return new JwtLoginResult(LoginStatus.AccountLocked);

            var result = await _signInManager.PasswordSignInAsync(login, password, isPersistent, false);
            if (!result.Succeeded)
                return new JwtLoginResult(LoginStatus.InvalidCredentials);
            
            var token = GenerateJwtToken(login, user);
            return new JwtLoginResult(token);
        }

        public async Task<JwtRegisterResult> JwtRegisterAsync(string email, string login, string password)
        {
            var user = new User
            {
                DisplayName = login,
                UserName = email,
                Email = email
            };

            if (await UserExistsAsync(email, login))
                return new JwtRegisterResult(RegisterStatus.UserAlreadyExists);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return new JwtRegisterResult(RegisterStatus.Failure);

            await _signInManager.SignInAsync(user, false);
            var token = GenerateJwtToken(email, user);
            return new JwtRegisterResult(token);
        }

        private string GenerateJwtToken(string login, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
