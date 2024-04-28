using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Responses
{
    public record ExcelResultRs
    {
        public byte[] Bytes { get; set; }
        public string Title { get; set; }
    }
}
