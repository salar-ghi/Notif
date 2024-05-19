namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class NotifLogConfiguration : IEntityTypeConfiguration<NotifLog>
{
    public void Configure(EntityTypeBuilder<NotifLog> builder)
    {
        BaseConfiguration<NotifLog>.Configure(builder);

        builder.HasKey(x => x.Id);


        builder.HasOne<Notif>(x => x.Notif).WithMany(e => e.NotifLogs).HasForeignKey(x => x.NotifId);

        builder.HasOne<Provider>(x => x.Provider).WithMany(e => e.NotifLog).HasForeignKey(z => z.ProviderId);

        builder.Property(x => x.NotifId).IsUnicode(false);
        builder.HasIndex(x => x.ProviderId).IsUnique(false);
    }
}
