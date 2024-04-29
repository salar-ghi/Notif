namespace Domain.Enums;

public enum NotifStatus : byte
{
    [Description("")]
    waiting = 1,

    [Description("")]
    Delivered = 2,

    [Description("")]
    failed_waiting = 3,

    //[Description("")]
    //waiting_ = 4,

    [Description("")]
    failed= 5,
}
