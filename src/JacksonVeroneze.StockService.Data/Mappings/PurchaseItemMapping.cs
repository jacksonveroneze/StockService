using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Data.Mappings
{
    public class PurchaseItemMapping : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.ToTable("purchase_item");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Amount)
                .IsRequired();

            builder.Property(c => c.Value)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.Version)
                .IsRequired();

            builder.HasOne(p => p.Purchase)
                .WithMany(b => b.Items)
                .IsRequired()
                .HasForeignKey("purchase_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
