using Microsoft.EntityFrameworkCore;
using 掲示板Webアプリ.Models;

namespace 掲示板Webアプリ.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Format> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    }
}
