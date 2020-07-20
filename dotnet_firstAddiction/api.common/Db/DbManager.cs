namespace api.common.Db
{
    public class DbManager : IDbManager
    {
        public DbManager(string server, string user, string password, string database)
        {
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Database = database;
        }

        public string Server { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public string CreateConnectionString()
        {
            return $"server={this.Server};uid={this.User};pwd={this.Password};database={this.Database}";
        }
    }
}