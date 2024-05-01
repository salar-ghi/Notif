namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class NotifLogConfiguration : IEntityTypeConfiguration<NotifLog>
{
    public void Configure(EntityTypeBuilder<NotifLog> builder)
    {
        BaseConfiguration<NotifLog>.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.NotifId).IsRequired();


        builder.HasOne(x => x.Notif).WithOne(e => e.NotifLog).HasForeignKey<NotifLog>(x => x.NotifId);
        builder.HasOne(x => x.Provider).WithOne(e => e.NotifLog).HasForeignKey<NotifLog>(x => x.ProviderId);
    }
}
