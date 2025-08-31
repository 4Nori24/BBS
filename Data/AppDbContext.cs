using Microsoft.EntityFrameworkCore;
using BBSWebApp.Models;

namespace BBSWebApp.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Format> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    }
}
