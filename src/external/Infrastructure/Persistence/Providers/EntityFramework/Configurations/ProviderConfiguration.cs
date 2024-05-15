
namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        BaseConfiguration<Provider>.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        //builder.HasMany(x => x.NotifLog).WithOne(j => j.Provider)
    }
}
