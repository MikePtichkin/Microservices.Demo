using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microservices.Demo.TestService.Common.Kafka.Consumer;

public static class KafkaJsonSerializer
{
    public static JsonSerializerOptions DefaultSettings
    {
        get
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
            };
            settings.Converters.Add(new JsonStringEnumConverter());
            return settings;
        }
    }
}
