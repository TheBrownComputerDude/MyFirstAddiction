namespace api.common.Security
{
    public interface IPasswordVerifier
    {
        PasswordResponse HashPassword(string password);
    }
}