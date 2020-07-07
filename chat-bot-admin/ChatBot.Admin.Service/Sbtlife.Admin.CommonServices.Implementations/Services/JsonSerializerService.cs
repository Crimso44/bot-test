using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ChatBot.Admin.CommonServices.Services.Abstractions;
using ChatBot.Admin.CommonServices.ContractResolvers;
using ChatBot.Admin.CommonServices.Converters;

namespace ChatBot.Admin.CommonServices.Services
{
    internal class JsonSerializerService : IJsonSerializerService
    {
        private static readonly JsonSerializer JsonSerializer;

        static JsonSerializerService()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new OptionalContractResolver(),
                Culture = CultureInfo.InvariantCulture,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            serializerSettings.Converters.Add(new OptionalJsonConverter());
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            JsonSerializer = JsonSerializer.Create(serializerSettings);
        }

        public string GetJsonString<T>(T data)
        {
            if (data == null)
                return null;

            var sb = new StringBuilder();

            using(var sw = new StringWriter(sb))
            {
                JsonSerializer.Serialize(sw, data, typeof(T));
                return sb.ToString();
            }
        }

        public T GetJsonObject<T>(string data)
        {
            if (data == null)
                return default(T);

            using (var sr = new StringReader(data))
            using (var jr = new JsonTextReader(sr))
            {
                return JsonSerializer.Deserialize<T>(jr);
            }
        }

        public object JObjectToObject(Type type, object data)
        {
            if (data == null)
                return null;

            if (data.GetType() != typeof(JObject))
                throw new ArgumentException(nameof(data));

            return ((JObject) data).ToObject(type, JsonSerializer);
        }
    }
}
