
namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        BaseConfiguration<Recipient>.Configure(builder);

        builder.Property(x => x.UserId).IsRequired();
    }
}
