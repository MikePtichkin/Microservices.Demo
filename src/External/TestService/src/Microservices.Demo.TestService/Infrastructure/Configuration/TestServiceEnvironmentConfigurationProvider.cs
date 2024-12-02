namespace Microservices.Demo.TestService;

public class TestServiceEnvironmentConfigurationProvider : ConfigurationProvider
{
    private const string KafkaBrokers = "DEMO_KAFKA_BROKERS";
    private const string OrdersConnectionString = "DEMO_ORDER_SERVICE_DB_CONNECTION_STRING";
    private const string CustomersConnectionString = "DEMO_CUSTOMER_SERVICE_DB_CONNECTION_STRING";
    private const string TimeoutSeconds = "DEMO_TEST_SERVICE_TIMEOUT_SECONDS";

    public override void Load()
    {
        var kafkaBrokers = Environment.GetEnvironmentVariable(KafkaBrokers);
        var ordersConnectionString = Environment.GetEnvironmentVariable(OrdersConnectionString);
        var customersConnectionString = Environment.GetEnvironmentVariable(CustomersConnectionString);
        var samplingDuration = Environment.GetEnvironmentVariable(TimeoutSeconds);

        Data = new Dictionary<string, string?>
        {
            ["Integrations:OrdersOutputEvents:Kafka:BootstrapServers"] = kafkaBrokers,
            ["Integrations:OrdersInputErrors:Kafka:BootstrapServers"] = kafkaBrokers,
            ["Data:Orders:ConnectionString"] = ordersConnectionString,
            ["Data:Customers:ConnectionString"] = customersConnectionString,
            ["Mismatch:SamplingDurationSec"] = samplingDuration,
        };
    }
}
