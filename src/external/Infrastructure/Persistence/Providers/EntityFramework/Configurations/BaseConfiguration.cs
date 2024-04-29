using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class BaseConfiguration<T> where T : notnull, EntityBase
{
    public static void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Ignore(e => e.ValidationErrors);
    }
}
