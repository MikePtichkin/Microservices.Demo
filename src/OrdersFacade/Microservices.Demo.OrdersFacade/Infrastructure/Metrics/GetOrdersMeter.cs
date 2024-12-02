using System.Diagnostics.Metrics;

namespace Microservices.Demo.OrdersFacade.Infrastructure.Metrics;

public class GetOrdersMeter : IGetOrdersMeter
{
    public const string MeterName = "get-orders-metrics";

    private readonly Counter<int> _ordersByCustomerTotal;
    private readonly Counter<int> _ordersByRegionTotal;

    private readonly Histogram<long> _ordersByCustomerPerRequest;
    private readonly Histogram<long> _ordersByRegionPerRequest;

    public GetOrdersMeter()
    {
        var meter = new Meter(MeterName);

        _ordersByCustomerTotal = meter.CreateCounter<int>("orders-by-customer-total");
        _ordersByRegionTotal = meter.CreateCounter<int>("get-orders-by-region-total");

        _ordersByCustomerPerRequest = meter.CreateHistogram<long>("orders-by-customer-per-request");
        _ordersByRegionPerRequest = meter.CreateHistogram<long>("orders-by-region-per-request");
    }

    public void IncrementOrdersByCustomerTotalCounter(int delta = 1) =>
        _ordersByCustomerTotal.Add(delta);
    public void RecordOrdersByCustomerPerRequest(long milliseconds) =>
        _ordersByCustomerPerRequest.Record(milliseconds);

    public void IncrementOrdersByRegionTotalCounter(int delta = 1) =>
        _ordersByRegionTotal.Add(delta);
    public void OrdersByRegionPerRequest(long milliseconds) =>
        _ordersByRegionPerRequest.Record(milliseconds);
}
