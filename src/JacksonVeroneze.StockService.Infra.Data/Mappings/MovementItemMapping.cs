using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public class MovementItemMapping : IEntityTypeConfiguration<MovementItem>
    {
        public void Configure(EntityTypeBuilder<MovementItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Amount)
                .IsRequired();

            builder.HasOne(p => p.Movement)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.AdjustmentItems)
                .WithMany(p => p.MovementItems);

            builder.HasMany(p => p.OutputItems)
                .WithMany(p => p.MovementItems);

            builder.HasMany(p => p.PurchaseItems)
                .WithMany(p => p.MovementItems);

            builder.ConfigureDefaultFiledsMapping();
        }
    }
}
