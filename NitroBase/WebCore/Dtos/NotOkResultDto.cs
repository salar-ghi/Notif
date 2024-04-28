using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebCore.Dtos
{
    public record NotOkResultDto : ResultBaseDto
    {
        public string Message { get; set; }
        public IEnumerable<string> Parameters { get; set; }

        public NotOkResultDto(string message, IEnumerable<string> parameters = null)
        {
            Message = message;
            Parameters = parameters;
        }
    }
}
