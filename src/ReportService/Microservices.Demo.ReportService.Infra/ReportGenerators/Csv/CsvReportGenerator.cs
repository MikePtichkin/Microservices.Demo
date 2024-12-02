using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using Microservices.Demo.ReportService.Bll.Contracts;
using Microservices.Demo.ReportService.Domain.Orders;
using Microservices.Demo.ReportService.Infra.Options;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal sealed class CsvReportGenerator : IReportGenerator
{
    private readonly CsvReportGeneratorOptions _options;
    private readonly IOrdersCient _ordersClient;

    public CsvReportGenerator(
        IOptions<CsvReportGeneratorOptions> options,
        IOrdersCient ordersCient)
    {
        _options = options.Value;
        _ordersClient = ordersCient;
    }

    private CsvConfiguration Configuration => new(CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
        ShouldQuote = args => false
    };

    public async Task<byte[]> GenerateReportCsvBytes(long customerId, CancellationToken cancellationToken)
    {
        var orderEntities = await _ordersClient.Query(customerId, cancellationToken);

        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, Configuration);

        var orderRecords = MapOrdersToCsvRecords(orderEntities);
        await csv.WriteRecordsAsync(orderRecords, cancellationToken);

        writer.Flush();
        memoryStream.Flush();

        
        return memoryStream.ToArray();
    }

    private OrderRecord[] MapOrdersToCsvRecords(IReadOnlyCollection<Order> orders)
    {
        var orderRecord = orders.Select(o => new OrderRecord(
            Id: o.Id,
            Status: o.Status.ToString(),
            Comment: o.Comment ?? "",
            CreatedAt: o.CreatedAt));

        return [.. orderRecord];
    }
}
