namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class BlackListConfiguration : IEntityTypeConfiguration<BlackList>
{
    public void Configure(EntityTypeBuilder<BlackList> builder)
    {
        BaseConfiguration<BlackList>.Configure(builder);

        builder.HasKey(x => x.Id);
    }
}
