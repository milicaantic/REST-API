using Praksa2.Models;
using Microsoft.EntityFrameworkCore;

namespace Praksa2.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
    }
}
