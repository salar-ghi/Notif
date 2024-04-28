using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum ExceptionCodeEnum: byte
    {
        UnauthorizedAccess = 50,
        InvalidOperation = 98,
        InvalidInput = 99,
        LimitationComponentError = 100,
        LimitationComponentValidationError = 101,
        InvalidOperationError = 102,
        OrderStatusError = 201,



    }
}
