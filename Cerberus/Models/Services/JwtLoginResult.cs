using System;
namespace Cerberus.Models.Services
{
    public class JwtLoginResult
    {
        public JwtLoginResult(LoginStatus status)
        {
            Status = status;
        }

        public LoginStatus Status { get; }
    }

    public enum LoginStatus
    {
        InvalidCredentials,
        AccountLocked,
        Success
    }
}
