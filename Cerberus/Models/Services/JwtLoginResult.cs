using System;
namespace Cerberus.Models.Services
{
    public class JwtLoginResult
    {
        public JwtLoginResult(LoginStatus status)
        {
            Status = status;
        }

        public JwtLoginResult(string token)
        {
            Status = LoginStatus.SUCCESS;
            Token = token;
        }

        public LoginStatus Status { get; set; }
        public string Token { get; set; }
    }

    public enum LoginStatus
    {
        INVALID_CREDENTIALS,
        ACCOUNT_LOCKED,
        SUCCESS
    }
}
