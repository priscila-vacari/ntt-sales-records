using Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Sales.Infra.MapConfigs
{
    [ExcludeFromCodeCoverage]
    public class SaleMapConfig: IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("sales");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Number)
                .HasColumnName("number")
                .IsRequired();

            builder.Property(p => p.Date)
                .HasColumnName("date")
                .IsRequired();

            builder.Property(p => p.Customer)
                .HasColumnName("customer")
                .IsRequired();

            builder.Property(p => p.TotalValue)
                .HasColumnName("totalValue")
                .IsRequired();

            builder.Property(p => p.Branch)
                .HasColumnName("branch")
                .IsRequired();

            builder.Property(p => p.IsCancelled)
                .HasColumnName("isCancelled")
                .IsRequired();
        }
    }
}
