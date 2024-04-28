namespace Domain.Enums;

public enum NotifType : byte
{
    [Description("پیامک")]
    SMS = 0,

    [Description("ایمیل")]
    Email = 1,

    [Description("نوتیف درون برنامه")]
    Signal = 2,

    [Description("تلگرام")]
    Telegram = 3,

    [Description("واتسآپ")]
    Whatsapp = 4,
}
