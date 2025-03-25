using Sales.Domain.Entities;
using Sales.Infra.MapConfigs;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Sales.Infra.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContextPostgre(DbContextOptions<AppDbContextPostgre> options) : DbContext(options)
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SaleMapConfig());
            modelBuilder.ApplyConfiguration(new SaleItemMapConfig());
        }
    }
}
