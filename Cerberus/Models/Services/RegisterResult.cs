namespace Cerberus.Models.Services
{
    public class RegisterResult
    {
        public RegisterResult(RegisterStatus status)
        {
            Status = status;
        }

        public RegisterStatus Status { get; }
    }

    public enum RegisterStatus
    {
        EmailInUse,
        DisplayNameInUse,
        LanguageNotExists,
        Failure,
        Success
    }
}
