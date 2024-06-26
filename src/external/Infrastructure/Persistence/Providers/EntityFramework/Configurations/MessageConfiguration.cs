﻿namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        BaseConfiguration<Message>.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Body).IsRequired().HasMaxLength(1000);

        builder.Property(n => n.status).IsRequired();
        builder.Property(n => n.Type).IsRequired();

        builder.Property(n => n.CreatedAt).IsRequired().HasDefaultValue(DateTime.UtcNow);
        //builder.Property(n => n.HangfireJobId).IsRequired(false);
        builder.Property(n => n.RowVersion).IsRowVersion();

        builder.Property(n => n.status).HasDefaultValue(NotifStatus.waiting);
        builder.Property(n => n.Attemp).HasDefaultValue(0);
        //builder.HasMany(x => x.Recipients).WithOne(e => e.Notif).IsRequired();

    }
}

