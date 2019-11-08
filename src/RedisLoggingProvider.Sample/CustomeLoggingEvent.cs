using Microsoft.Extensions.Logging.RedisProvider;
using Newtonsoft.Json;

namespace RedisLoggingProvider.Sample
{
    public class CustomeLoggingEvent: BaseEventData
    {
        [JsonIgnore]
        public string CompanyCode { get; set; }
    }
}
