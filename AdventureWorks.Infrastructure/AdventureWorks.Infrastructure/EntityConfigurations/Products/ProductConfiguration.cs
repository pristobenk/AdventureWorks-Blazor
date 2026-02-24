using AdventureWorks.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdventureWorks.Infrastructure.EntityConfigurations.Products;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProductNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(p => p.Color)
            .HasMaxLength(15);

        builder.Property(p => p.Size)
            .HasMaxLength(5);

        builder.Property(p => p.SizeUnitMeasureCode)
            .HasMaxLength(3);

        builder.Property(p => p.WeightUnitMeasureCode)
            .HasMaxLength(3);

        builder.Property(p => p.StandardCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ListPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Weight)
            .HasColumnType("decimal(8,2)");

        // Ensure DateTime values are stored/treated as UTC
        builder.Property(p => p.SellStartDate)
            .HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.Property(p => p.SellEndDate)
            .HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.Property(p => p.DiscontinuedDate)
            .HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.Property(p => p.ModifiedDate)
            .HasConversion(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
    }
}

