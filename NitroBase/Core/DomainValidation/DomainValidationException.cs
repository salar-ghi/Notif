using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.DomainValidation.Models;

namespace Core.DomainValidation
{

    [Serializable]
    /// <summary>
    /// This exception will be thrown when a domain validation error occurs
    /// </summary>
    public class DomainValidationException : Exception
    {
        public const string const_Messages_DomainValidationError = "{0} validation failed, because {1} value is not valid.";
        protected IEnumerable<DomainValidationError> ValidationErrors { get; set; }

        public override IDictionary Data => ValidationErrors.ToDictionary(e => $"[{e.Level.ToString().ToUpper()}]: {e.Message} ({e.Code}) \r\n");

        public DomainValidationException() { }
        public DomainValidationException(params DomainValidationError[] validationErrors) : base()
        {
            ValidationErrors = validationErrors;
        }
        public DomainValidationException(string message) : base(message) { }
        public DomainValidationException(string message, Exception inner) : base(message, inner) { }
        protected DomainValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
