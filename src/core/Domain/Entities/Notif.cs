namespace Domain.Entities;

public class Notif : EntityBase, IValidate, ITrackable
{
    
    public long Id { get; set; } = default(long);

    public NotifType Type { get; set; } = default(NotifType); // e.g., SMS, Email, RabbitMQ
    public int SenderId { get; set; } // ??????????
    //public virtual User Sender { get; set; }


    public string Title { get; set; } = default!;    
    public MessageType MessageType { get; set; }
    public string Message { get; set; } = default!;

    [DefaultValue(NotifStatus.Delivered)]
    public NotifStatus status { get; set; }

    public bool IsSent { get; set; } 
    public int Attemp { get; set; }
    public DateTime NextTry { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
    public int ProviderID { get; set; }
    public virtual ICollection<Recipient> Recipients { get; set; }
    
    public virtual ICollection<NotifLog> NotifLogs { get; set; }

    public void Validate()
    {
        #region Message
        if (string.IsNullOrEmpty(Title))
            _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(Title)));
        #endregion

        #region Description
        if (string.IsNullOrEmpty(Message))
            _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(Message)));

        #endregion

        #region NotifType

        #endregion
    }
}
