using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainValidation.Models
{
    public class DomainValidationError
    {

        public LogLevel Level { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

    }
}
