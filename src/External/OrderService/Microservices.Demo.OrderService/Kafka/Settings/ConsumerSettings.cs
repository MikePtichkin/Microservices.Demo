namespace Microservices.Demo.OrderService.Kafka.Settings;

public class ConsumerSettings
{
    public string Topic { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;

    public bool AutoCommit { get; set; } = false;
}