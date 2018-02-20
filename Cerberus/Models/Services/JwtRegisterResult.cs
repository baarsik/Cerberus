using System;
namespace Cerberus.Models.Services
{
    public class JwtRegisterResult
    {
        public JwtRegisterResult(RegisterStatus status)
        {
            Status = status;
        }

        public JwtRegisterResult(string token)
        {
            Status = RegisterStatus.SUCCESS;
            Token = token;
        }

        public RegisterStatus Status { get; set; }
        public string Token { get; set; }
    }

    public enum RegisterStatus
    {
        USER_ALREADY_EXISTS,
        FAILURE,
        SUCCESS
    }
}
