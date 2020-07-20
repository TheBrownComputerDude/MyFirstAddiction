namespace api.common.Db
{
    public interface IDbManager
    {
        string Server { get; set; }

        string User { get; set; }

        string Password { get; set; }

        string Database { get; set; }

        string CreateConnectionString();
    }
}