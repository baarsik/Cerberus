using System;
namespace Cerberus.Models.Services
{
    public class JwtRegisterResult
    {
        public JwtRegisterResult(RegisterStatus status)
        {
            Status = status;
        }

        public RegisterStatus Status { get; }
    }

    public enum RegisterStatus
    {
        UserAlreadyExists,
        Failure,
        Success
    }
}
