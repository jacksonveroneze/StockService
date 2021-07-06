using JacksonVeroneze.StockService.Core.DomainObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksonVeroneze.StockService.Infra.Data.Mappings
{
    public static class DefaultFiledsMapping
    {
        public static void ConfigureDefaultFiledsMapping<T>(this EntityTypeBuilder<T> builder) where T : Entity
        {
            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.Version)
                .IsConcurrencyToken()
                .IsRequired();

            builder.Property(c => c.TenantId)
                .IsRequired();
        }
    }
}
