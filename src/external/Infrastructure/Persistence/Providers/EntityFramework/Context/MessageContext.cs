namespace Infrastructure.Persistence.Providers.EntityFramework.Context;

public partial class MessageContext : DbContext
{
    public MessageContext() { }

    public MessageContext(DbContextOptions options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        this.ChangeTracker.LazyLoadingEnabled = false;
    }



    public virtual DbSet<Message> Notifs { get; set; }
    public virtual DbSet<BlackList> BlackList { get; set; }
    public virtual DbSet<MessageLog> NotifLog { get; set; }
    public virtual DbSet<Provider> Provider { get; set; }
    //public virtual DbSet<ProviderSetting> ProviderConfigurations { get; set; }
    public virtual DbSet<Recipient> Recipient { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new BlackListConfiguration());
        modelBuilder.ApplyConfiguration(new MessageLogConfiguration());
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
