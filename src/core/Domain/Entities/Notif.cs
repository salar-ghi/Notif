namespace Domain.Entities;

public class Notif : EntityBase, IValidate, ITrackable
{
    //public readonly ILazyLoader _lazyLoader;
    //public Notif()
    //{
        
    //}
    public long Id { get; private set; } = default(long);

    public NotifType Type { get; set; } = default(NotifType); // e.g., SMS, Email, RabbitMQ
    public int SenderId { get; set; } // ??????????
    //public virtual User Sender { get; set; }


    public string Title { get; set; } = default!;    
    public MessageType MessageType { get; set; }
    public string Message { get; set; } = default!;
    public NotifStatus status { get; set; } = default(NotifStatus);


    // hangfire
    #region Hangfire

    public string HangfireJobId { get; set; } = default!;
    public bool IsSent { get; set; } // for hangfire job
    public DateTime NextTry { get; set; } = DateTime.UtcNow;
    [Timestamp]
    public byte[] RowVersion { get; set; } // ??? for cuncurrency
    #endregion


    public DateTime CreatedAt { get; set; }
    public long CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }

    public virtual ICollection<Recipient> Recipients { get; set; }
    //private ICollection<Recipient> _recipients;
    //public virtual ICollection<Recipient> Recipients
    //{ 
    //    get => _lazyLoader?.Load(this, ref _recipients); 
    //    set => _recipients = value; 
    //}
    public virtual NotifLog NotifLog { get; set; }

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
