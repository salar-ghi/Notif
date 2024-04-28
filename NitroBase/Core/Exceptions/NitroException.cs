using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Exceptions
{
    public class NitroException : Exception
    {
        public NitroException()
        {
        }
        public NitroException(ExceptionCodeEnum errorCode,
                              string customMessage = null,
                              IDictionary<string, string> data = null,
                              Exception innerException = null) : base(customMessage, innerException)
        {
            ErrorCode = errorCode;
            CustomMessage = customMessage;

            if (data is not null)
                foreach (var item in data)
                    this.Data.Add(item.Key, item.Value);

        }

        public ExceptionCodeEnum ErrorCode { get; set; }

        public string CustomMessage { get; set; }

        public object Value { get; set; }

    }
}
