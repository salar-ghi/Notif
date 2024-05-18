namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class NotifConfiguration : IEntityTypeConfiguration<Notif>
{
    public void Configure(EntityTypeBuilder<Notif> builder)
    {
        BaseConfiguration<Notif>.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Message).IsRequired().HasMaxLength(1000);

        builder.Property(n => n.status).IsRequired();

        builder.Property(n => n.CreatedAt).IsRequired().HasDefaultValue(DateTime.UtcNow);
        //builder.Property(n => n.HangfireJobId).IsRequired(false);
        builder.Property(n => n.RowVersion).IsRowVersion();

        builder.Property(n => n.status).HasDefaultValue(NotifStatus.waiting);

        //builder.HasMany(x => x.Recipients).WithOne(e => e.Notif).IsRequired();

    }
}

