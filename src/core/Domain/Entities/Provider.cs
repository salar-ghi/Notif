﻿namespace Domain.Entities;

public class Provider : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsEnabled { get; set; }
    public string JsonConfig { get; set; } = default!;
    public ProviderType Type { get; set; }
    public byte Priority { get; set; }
    //public NotifType Type { get; set; } // e.g., SMS, Email, RabbitMQ
    public virtual NotifLog NotifLog { get; set; }


    //.... Another Properties and Configurations
    //public List<ProviderSetting> config { get; set; } = new List<ProviderSetting>();


}
