using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using Microservices.Demo.ReportService.Domain.Orders;
using Microservices.Demo.ReportService.Infra.Options;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Infra.UnitTests.CsvReportGeneratorTests;

public class CsvReportGeneratorTests
{
    private readonly CsvReportGenerator _csvReportGenerator;
    private readonly IOrdersCient _ordersClientMock;

    public CsvReportGeneratorTests()
    {
        var options = Substitute.For<IOptions<CsvReportGeneratorOptions>>();
        options.Value.Returns(new CsvReportGeneratorOptions { CsvLineProcessingDelayMs = 0 });

        _ordersClientMock = Substitute.For<IOrdersCient>();

        _csvReportGenerator = new CsvReportGenerator(options, _ordersClientMock);
    }

    [Fact]
    public async Task GenerateReportCsvBytes_ShouldReturnCorrectCsvBytes()
    {
        // Arrange
        var customerId = 1L;
        var ordersInitial = new[]
        {
            new Order(
                id: 1L,
                status: OrderStatus.New,
                createdAt: new TimeStamp { Value = DateTimeOffset.Now },
                comment: "first order"),
            new Order(
                id: 2L,
                status: OrderStatus.Delivered,
                createdAt: new TimeStamp { Value = DateTimeOffset.Now },
                comment: "second order")
        };

        _ordersClientMock
            .Query(customerId, Arg.Any<CancellationToken>())
            .Returns(ordersInitial);

        // Act
        var result = await _csvReportGenerator.GenerateReportCsvBytes(customerId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        using var stream = new MemoryStream(result);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

        var records = csv.GetRecords<OrderRecord>().ToArray();

        Assert.Equal(ordersInitial.Length, records.Length);

        var orderExpected0 = ordersInitial[0];
        Assert.Equal(orderExpected0.Id, records[0].Id);
        Assert.Equal(orderExpected0.Status.ToString(), records[0].Status);
        Assert.Equal(orderExpected0.Comment!, records[0].Comment);

        var orderExpected1 = ordersInitial[1];
        Assert.Equal(orderExpected1.Id, records[1].Id);
        Assert.Equal(orderExpected1.Status.ToString(), records[1].Status);
        Assert.Equal(orderExpected1.Comment!, records[1].Comment);
    }
}