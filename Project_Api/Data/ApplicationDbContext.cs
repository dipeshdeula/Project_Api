using Microsoft.EntityFrameworkCore;
using Project_Api.Models;

namespace Project_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
        public DbSet<Product> Products { get; set; }
    }
}
