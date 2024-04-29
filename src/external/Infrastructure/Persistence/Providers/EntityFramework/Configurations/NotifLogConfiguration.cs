using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class NotifLogConfiguration : IEntityTypeConfiguration<NotifLog>
{
    public void Configure(EntityTypeBuilder<NotifLog> builder)
    {
        throw new NotImplementedException();
    }
}
