using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Data.Mappings
{
    public class PurchaseMapping : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("purchase");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.Version)
                .IsRequired();
        }
    }
}
