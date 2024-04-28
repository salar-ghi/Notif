using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.DomainValidation.Helpers;
using Core.DomainValidation.Models;
using Core.Enums;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Services
{
    public class Guard : IGuard
    {

        public Guard()
        {

        }

        public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message = "", IDictionary<string, string> dataCollection = null)
        {
            dataCollection ??= new Dictionary<string, string>();

            if (!trueCondition)
            {
                //if (message == "$translate")
                //    message = TranslateService.Value.GetTranslate(exceptionCode.ToString());
                //else
                message = message.Equals(string.Empty) ? exceptionCode.ToString() : message;

                Exception exception = null;
                switch (exceptionCode)
                {
                    case ExceptionCodeEnum.InvalidInput:
                        exception = new DomainValidation.DomainValidationException(
                            ErrorsHelper.RaiseMessage(message, Microsoft.Extensions.Logging.LogLevel.Error)
                            );
                        break;
                    case ExceptionCodeEnum.UnauthorizedAccess:
                    case ExceptionCodeEnum.InvalidOperation:
                    case ExceptionCodeEnum.LimitationComponentError:
                    case ExceptionCodeEnum.LimitationComponentValidationError:
                    case ExceptionCodeEnum.InvalidOperationError:
                    case ExceptionCodeEnum.OrderStatusError:
                    default:
                        exception = new NitroException(exceptionCode, message) { };
                        foreach (var item in dataCollection)
                            exception.Data.Add(item.Key, item.Value);

                        break;
                }


                throw exception;
            }
        }

        public void Assert(bool trueCondition, ExceptionCodeEnum exceptionCode, string message = "", params object[] dataCollection)
        {
            var dataDic = new Dictionary<string, string>();
            if (dataCollection is not null)
                foreach (var item in dataCollection)
                    dataDic.Add(nameof(item), item.ToJsonString());

            Assert(trueCondition, exceptionCode, message, dataDic);
        }
    }
}
