using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
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

            builder.HasOne(p => p.Adjustment)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsAdjustment)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureDefaultFiledsMapping();
        }
    }
}
