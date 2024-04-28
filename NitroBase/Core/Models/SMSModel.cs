using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SMSRequestModel
    {
        public IEnumerable<string> To { get; set; }
        public string Message { get; set; }
    }
}
