using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SaleId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.ProductTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.DiscountPercent)
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        builder.Property(x => x.DiscountAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.TotalItemAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.Cancelled)
            .IsRequired();

        builder.HasIndex(x => x.SaleId);
        builder.HasIndex(x => x.ProductId);
    }
}
