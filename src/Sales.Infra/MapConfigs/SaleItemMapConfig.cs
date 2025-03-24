using Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Sales.Infra.MapConfigs
{
    [ExcludeFromCodeCoverage]
    public class SaleItemMapConfig : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("sale_items");

            builder.HasKey(k => new { k.SaleId, k.ProductId });

            builder.Property(p => p.ProductId)
                .HasColumnName("productId")
                .IsRequired();

            builder.Property(p => p.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(p => p.UnitPrice)
                .HasColumnName("unitPrice")
                .IsRequired();

            builder.Property(p => p.Discount)
                .HasColumnName("discount")
                .IsRequired();

            builder.Property(p => p.TotalValue)
                .HasColumnName("totalValue")
                .IsRequired();

            builder.HasOne(p => p.Sale)
                   .WithMany(p => p.Items)
                   .HasForeignKey(p => p.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
