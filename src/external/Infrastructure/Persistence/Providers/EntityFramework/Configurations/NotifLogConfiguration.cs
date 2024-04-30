namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class NotifLogConfiguration : IEntityTypeConfiguration<NotifLog>
{
    public void Configure(EntityTypeBuilder<NotifLog> builder)
    {
        BaseConfiguration<NotifLog>.Configure(builder);
    }
}
