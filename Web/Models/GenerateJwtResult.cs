using System;
using DataContext.Models;

namespace Web.Models
{
    public class GenerateJwtResult
    {
        public GenerateJwtResult(bool success)
        {
            if (success)
                throw new ArgumentException("Token is not provided, use another constructor");

            Success = false;
            Token = string.Empty;
        }
        
        public GenerateJwtResult(ApplicationUser user, string token)
        {
            Success = true;
            User = user;
            Token = token;
        }
        
        public bool Success { get; private set; }
        public string Token { get; private set; }
        public ApplicationUser User { get; private set; }
    }
}