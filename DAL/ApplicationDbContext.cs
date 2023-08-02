using Project.Entities;
using Microsoft.EntityFrameworkCore;

namespace Project.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  { }

        public DbSet<Users> Users { get; set; }
    }
}
