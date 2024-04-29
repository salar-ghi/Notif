namespace Infrastructure.Persistence.Providers.EntityFramework.Context;

public partial class NotifContext : DbContext
{
    public NotifContext() { }

    public NotifContext(DbContextOptions options) : base(options) { }



    public virtual DbSet<Notif> Notifs { get; set; }
    public virtual DbSet<BlackList> BlackLists { get; set; }
    public virtual DbSet<NotifLog> NotifLogs { get; set; }
    public virtual DbSet<Provider> Providers { get; set; }
    //public virtual DbSet<ProviderSetting> ProviderConfigurations { get; set; }
    public virtual DbSet<Recipient> Recipients { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //modelBuilder.Entity<Notif>()
        //    .HasDiscriminator<string>("NotifType")
        //    .HasValue<SmsNotif>("SMS");

        //modelBuilder.Entity<Provider>()
        //    .HasDiscriminator<string>("ProviderType")
        //    .HasValue<SmsProvider>("SMS")
        //    .HasValue<EmailProvider>("Email")
        //    .HasValue<RabbitMqProvider>("RabbitMQ");

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        base.OnConfiguring(optionsBuilder);
    }



}
