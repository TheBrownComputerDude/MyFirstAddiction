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

        public DbSet<User> User { get; set; }

        public DbSet<Video> Video { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(this.Manager.CreateConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e => 
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Username).IsRequired();
                e.Property(u => u.Email).IsRequired();
                e.Property(u => u.Password).IsRequired();
                e.Property(u => u.Salt).IsRequired();
            });
            modelBuilder.Entity<UserInfo>(e => 
            {
                e.HasKey(u => u.Id);
            });

            modelBuilder.Entity<Video>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Location).IsRequired();
                e.HasIndex(u => u.Location).IsUnique();
                e.HasOne(u => u.User)
                    .WithMany(u => u.Videos);
            });
        }
    }
}