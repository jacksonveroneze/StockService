using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public class DevolutionItemMapping : IEntityTypeConfiguration<DevolutionItem>
    {
        public void Configure(EntityTypeBuilder<DevolutionItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Amount)
                .IsRequired();

            builder.HasOne(p => p.Devolution)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsDevolution)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ConfigureDefaultFiledsMapping();
        }
    }
}
