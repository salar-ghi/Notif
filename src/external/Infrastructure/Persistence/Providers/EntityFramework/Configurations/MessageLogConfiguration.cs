namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class MessageLogConfiguration : IEntityTypeConfiguration<MessageLog>
{
    public void Configure(EntityTypeBuilder<MessageLog> builder)
    {
        BaseConfiguration<MessageLog>.Configure(builder);

        builder.HasKey(x => x.Id);


        builder.HasOne<Message>(x => x.Message).WithMany(e => e.MessageLogs).HasForeignKey(x => x.MessageId);

        builder.HasOne<Provider>(x => x.Provider).WithMany(e => e.MessageLog).HasForeignKey(z => z.ProviderId);

        builder.Property(x => x.MessageId).IsUnicode(false);
        builder.HasIndex(x => x.ProviderId).IsUnique(false);
    }
}
