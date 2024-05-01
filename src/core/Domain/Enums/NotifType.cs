namespace Domain.Enums;

public enum NotifType : byte
{
    [Description("پیامک")]
    SMS = 1,

    [Description("ایمیل")]
    Email = 2,

    [Description("نوتیف درون برنامه")]
    Signal = 3,

    [Description("")]
    MessageBrocker = 4,

    [Description("تلگرام")]
    Telegram = 5,

    [Description("واتسآپ")]
    Whatsapp = 6,
}
