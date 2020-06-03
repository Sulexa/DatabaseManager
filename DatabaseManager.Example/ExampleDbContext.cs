using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MigrationManager.Example
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pseudo { get; set; }
    }
}
