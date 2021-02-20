using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Data.Mappings
{
    public class OutputMapping : IEntityTypeConfiguration<Output>
    {
        public void Configure(EntityTypeBuilder<Output> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.Property(c => c.State)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.Version)
                .IsRequired();
        }
    }
}
