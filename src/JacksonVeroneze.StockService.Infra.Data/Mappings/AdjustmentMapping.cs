using JacksonVeroneze.StockService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public class AdjustmentMapping : IEntityTypeConfiguration<Adjustment>
    {
        public void Configure(EntityTypeBuilder<Adjustment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.Property(c => c.State)
                .IsRequired();

            builder.ConfigureDefaultFiledsMapping();

            builder.Ignore(x => x.TotalValue);
        }
    }
}
