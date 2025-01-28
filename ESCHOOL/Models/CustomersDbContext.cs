using Microsoft.EntityFrameworkCore;

namespace ESCHOOL.Models
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
        {

        }
        public DbSet<Customers> Customers { get; set; }

        public DbSet<UsersChat> UsersChat { get; set; }
    }
}
