using Microsoft.EntityFrameworkCore;
using SnackShare.Api.Data.Entities;
using SnackShare.Api.Data.Entities.Products;
using SnackShare.Api.Data.Entities.Sales;

namespace SnackShare.Api.Data
{
    /// <summary>
    /// DB context for accessing database
    /// </summary>
    public class SnackDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductStock> ProductStock { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        public SnackDbContext(DbContextOptions<SnackDbContext> options)
            : base(options)
        {
        }
    }
}
