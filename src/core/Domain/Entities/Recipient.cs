﻿namespace Domain.Entities;

public class Recipient : EntityBase
{
    public long Id { get; set; }
    public long UserId { get; set; } = default!;
    public string Destination { get; set; } = default!;

    public long MessageId { get; set; }
    public virtual Message Message { get; set; } //???

}
