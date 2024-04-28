using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Constants;

namespace WebCore.Dtos
{
    public record OkResult<T> : ResultBaseDto where T : class
    {
        public T Result { get; set; }

        public OkResult(T result)
        {
            if (result is null)
                ArgumentNullException.ThrowIfNull("result");

            Result = result;
        }

    }

    public record OkListResult<T> : ResultBaseDto where T : class
    {
        public ICollection<T> Result { get; set; }

        public int PageSize { get; set; } = GlobalConstants.DefaultPageSize;
        public int PageNumber { get; set; } = 1;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public OkListResult(T result)
        {
            if (result is null)
                ArgumentNullException.ThrowIfNull("result");

            Result.Add(result);
        }

        public OkListResult(ICollection<T> result)
        {
            if (result is null)
                ArgumentNullException.ThrowIfNull("result");

            Result = result;
        }
    }
}
