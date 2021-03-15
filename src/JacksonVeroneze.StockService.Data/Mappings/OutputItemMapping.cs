using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Data.Mappings
{
    public class OutputItemMapping : IEntityTypeConfiguration<OutputItem>
    {
        public void Configure(EntityTypeBuilder<OutputItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Amount)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasPrecision(10,2)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.Version)
                .IsRequired();

            builder.Property(c => c.TenantId)
                .IsRequired();

            builder.HasOne(p => p.Output)
                .WithMany(b => b.Items)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(b => b.ItemsOutput)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
