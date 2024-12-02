namespace Microservices.Demo.OrdersFacade.Infrastructure.Metrics;

public interface IGetOrdersMeter
{
    void IncrementOrdersByCustomerTotalCounter(int delta = 1);
    void RecordOrdersByCustomerPerRequest(long milliseconds);

    void IncrementOrdersByRegionTotalCounter(int delta = 1);
    void OrdersByRegionPerRequest(long milliseconds);
}
