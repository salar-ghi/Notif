using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException() { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, params object[] parameters) : base(message)
        {
            foreach (var parameter in parameters)
                Data.Add(nameof(parameter), parameter);

        }
        public ForbiddenException(string message, Exception inner) : base(message, inner) { }
        protected ForbiddenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
