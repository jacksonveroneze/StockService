using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Data.Mappings
{
    public class AdjustmentItemMapping : IEntityTypeConfiguration<AdjustmentItem>
    {
        public void Configure(EntityTypeBuilder<AdjustmentItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

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

            builder.HasOne(p => p.Adjustment)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsAdjustment)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
