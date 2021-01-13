namespace Web.Models.Services
{
    public class LoginResult
    {
        public LoginResult(LoginStatus status)
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
