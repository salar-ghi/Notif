using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Core.Helpers
{
    public static class JsonHelper
    {
        private static Newtonsoft.Json.JsonSerializerSettings _newtonSoftSettings => new Newtonsoft.Json.JsonSerializerSettings
        {
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
            DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
            MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Default,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
        };

        private static System.Text.Json.JsonSerializerOptions _SystemTextSettings => new System.Text.Json.JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = true,

            //PropertyNamingPolicy = JsonNamingPolicy.()
        };

        public static async Task<System.IO.Stream> ToJsonStream(System.IO.Stream stream, object obj)
        {
            if (obj == null) return null;

            try
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(stream, obj, _SystemTextSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stream;
        }

        public static string ToJsonString(object obj, System.Text.Json.Serialization.JsonConverter[] jsonConverters = null)
        {
            if (obj == null) return string.Empty;

            try
            {
                if (jsonConverters is not null)
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), systemTextSettings);

                }

                return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), _SystemTextSettings);
                //return Utf8Json.JsonSerializer.ToJsonString(obj);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _newtonSoftSettings);
            }
        }

        public static T FromJsonString<T>(string json, System.Text.Json.Serialization.JsonConverter[] jsonConverters = null)
        {

            try
            {
                if (jsonConverters is not null)
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return System.Text.Json.JsonSerializer.Deserialize<T>(json, systemTextSettings);
                }

                return System.Text.Json.JsonSerializer.Deserialize<T>(json, _SystemTextSettings);
                //return Utf8Json.JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, _newtonSoftSettings);
            }
        }

        public static string GetItemByPathAsString(string jsonData, string jsonPath)
        {
            var result = GetItemByPath(jsonData, jsonPath);
            return (string)result;
        }

        public static JToken GetItemByPath(string jsonData, string jsonPath)
        {
            var root = Newtonsoft.Json.Linq.JToken.Parse(jsonData);
            return root.SelectToken(jsonPath);
        }

        public static IEnumerable<string> GetItemsByPath(string jsonData, string jsonPath)
        {
            var root = Newtonsoft.Json.Linq.JToken.Parse(jsonData);
            return root.SelectToken(jsonPath).Select(e => (string)e).ToList();
        }
    }

}
