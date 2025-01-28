using Microsoft.EntityFrameworkCore;

namespace ESCHOOL.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }
        public DbSet<SchoolInfo> SchoolInfo { get; set; }
       
    }
}
