namespace Infrastructure.Persistence.Providers.EntityFramework.Context;

public partial class NotifContext : DbContext
{
    public NotifContext() { }

    public NotifContext(DbContextOptions options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        this.ChangeTracker.LazyLoadingEnabled = false;
    }



    public virtual DbSet<Notif> Notifs { get; set; }
    public virtual DbSet<BlackList> BlackList { get; set; }
    public virtual DbSet<NotifLog> NotifLog { get; set; }
    public virtual DbSet<Provider> Provider { get; set; }
    //public virtual DbSet<ProviderSetting> ProviderConfigurations { get; set; }
    public virtual DbSet<Recipient> Recipient { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        modelBuilder.ApplyConfiguration(new NotifConfiguration());
        modelBuilder.ApplyConfiguration(new BlackListConfiguration());
        modelBuilder.ApplyConfiguration(new NotifLogConfiguration());
        modelBuilder.ApplyConfiguration(new ProviderConfiguration());
        modelBuilder.ApplyConfiguration(new RecipientConfiguration());

        base.OnModelCreating(modelBuilder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseLazyLoadingProxies(true);
        //optionsBuilder.UseChangeTrackingProxies();

        base.OnConfiguring(optionsBuilder);
    }



}
