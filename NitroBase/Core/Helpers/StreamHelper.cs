using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public sealed partial class StreamHelper
    {
        public static Task<bool> TryGetString(Stream stream, out string result)
        {
            result = "";

            if (stream == null)
                return Task.FromResult(false);

            if (!stream.CanRead)
                return Task.FromResult(false);

            try
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    result = reader.ReadToEnd();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
