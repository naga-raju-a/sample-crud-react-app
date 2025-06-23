using Microsoft.EntityFrameworkCore;
using Sample.Web.API.Models;

namespace Sample.Web.API.Services
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employee { get; set; }

        public DbSet<Cafe> Cafe { get; set; }
    }
}
