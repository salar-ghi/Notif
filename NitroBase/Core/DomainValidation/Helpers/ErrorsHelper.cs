using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainValidation.Models;

namespace Core.DomainValidation.Helpers
{
    public class ErrorsHelper
    {

        public static DomainValidationError FormatError(string propertyName) =>
            new DomainValidationError()
            {
                Code = 100,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Message = $"{propertyName} format is invalid."
            };

        public static DomainValidationError EmptyError(string propertyName) =>
            new DomainValidationError()
            {
                Code = 200,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Message = $"{propertyName} is null or empty."
            };

        public static DomainValidationError MinLengthError(string propertyName) =>
            new DomainValidationError()
            {
                Code = 300,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Message = $"{propertyName} is less than min value."
            };

        public static DomainValidationError MaxLengthError(string propertyName) =>
            new DomainValidationError()
            {
                Code = 301,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Message = $"{propertyName} is less than min value."
            };
        public static DomainValidationError RaiseMessage(string message, Microsoft.Extensions.Logging.LogLevel level) =>
            new DomainValidationError()
            {
                Code = 999,
                Level = level,
                Message = message,
            };
    }

}
