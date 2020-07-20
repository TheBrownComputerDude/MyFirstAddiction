using api.common.Db;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class FirstAddictionContext : DbContext
    {
        public FirstAddictionContext(IDbManager manager)
        {
            this.Manager = manager;
        }

        private IDbManager Manager { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(this.Manager.CreateConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}