namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

internal class NotifConfiguration : IEntityTypeConfiguration<Notif>
{
    public void Configure(EntityTypeBuilder<Notif> builder)
    {
        BaseConfiguration<Notif>.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Message).IsRequired().HasMaxLength(1000);

    }
}
