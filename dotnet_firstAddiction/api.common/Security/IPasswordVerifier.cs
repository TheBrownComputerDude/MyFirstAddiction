namespace api.common.Security
{
    public interface IPasswordVerifier
    {
        PasswordResponse HashPassword(string password);

        bool Validate(string checkPassword, string salt, string password);
    }
}