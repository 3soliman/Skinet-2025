using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.PictureUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.QuantityInStock)
            .HasDefaultValue(0);
    }
}

