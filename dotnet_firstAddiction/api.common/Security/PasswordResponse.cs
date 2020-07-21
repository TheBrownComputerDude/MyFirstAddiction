namespace api.common.Security
{
    public class PasswordResponse
    {
        public string Hash { get; set; }

        public string Salt { get; set; }
    }
}