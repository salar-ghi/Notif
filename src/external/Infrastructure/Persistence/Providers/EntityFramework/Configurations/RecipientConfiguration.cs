
namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        BaseConfiguration<Recipient>.Configure(builder);

        builder.HasKey(x  => x.Id);
        builder.Property(x => x.UserId).IsRequired();


        builder.HasOne(x => x.Message).WithMany(e => e.Recipients).IsRequired().HasForeignKey(x => x.MessageId);
    }
}
