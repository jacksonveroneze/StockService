using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public class PurchaseItemMapping : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Amount)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasPrecision(10,2)
                .IsRequired();

            builder.HasOne(p => p.Purchase)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsPurchase)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureDefaultFiledsMapping();
        }
    }
}
