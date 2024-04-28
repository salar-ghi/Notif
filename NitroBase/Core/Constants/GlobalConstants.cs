using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Constants
{
    public sealed class GlobalConstants
    {
        public const string ApiHealthCheckRoute = "/health";
        public const int DefaultPageSize = 25;
        public const string CorrelationIdKey = "CorrelationId";
        public const string PriceSqlDataType = "decimal(15,2)";


        public const string Message_InternalServerError = "خطایی در برنامه رخ داده است.";
        public const string Message_AccessDenied_Message = "با عرض پوزش،اجرای این درخواست مجاز نمی باشد.";
        
        public static TimeZoneInfo GetTehranTimeZoneInfo() => TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
        

        
    }
}
