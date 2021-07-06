using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public class MovementMapping : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsMovement)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureDefaultFiledsMapping();
        }
    }
}
