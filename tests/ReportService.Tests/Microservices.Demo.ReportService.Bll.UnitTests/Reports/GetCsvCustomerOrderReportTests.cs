using NSubstitute;
using Microservices.Demo.ReportService.Bll.Contracts;
using Microservices.Demo.ReportService.Bll.Reports.GetCsvCustomerOrdersReport;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Bll.UnitTests.Reports;

public class GetCsvCustomerOrderReportTests
{
    private static readonly GetCsvCustomerOrdersReportQuery Query = new(1L);

    private readonly GetCsvCustomerOrdersReportHandler _handler;

    private IRateLimiter _rateLimiterMock;
    private IReportGenerator _reportGeneratorMock;

    public GetCsvCustomerOrderReportTests()
    {
        _rateLimiterMock = new TestRateLimiter();
        _reportGeneratorMock = Substitute.For<IReportGenerator>();

        _handler = new GetCsvCustomerOrdersReportHandler(
            _rateLimiterMock,
            _reportGeneratorMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnCsvReport_WhenInvoked()
    {
        // Arrange
        var reportGeneratorMockExpectedCallsNumber = 1;
        var expectedCsvBytes = new byte[] { 0x1, 0x2, 0x3 };
        _reportGeneratorMock.GenerateReportCsvBytes(Query.CustomerId, Arg.Any<CancellationToken>())
            .Returns(expectedCsvBytes);

        // Act
        var result = await _handler.Handle(Query, CancellationToken.None);

        // Assert result
        Assert.NotNull(result);
        Assert.Equal(expectedCsvBytes, result);

        // Assert interaction
        await _reportGeneratorMock
            .Received(reportGeneratorMockExpectedCallsNumber)
            .GenerateReportCsvBytes(Query.CustomerId, Arg.Any<CancellationToken>());

        ((TestRateLimiter)_rateLimiterMock).WaitAsyncCalledOnce();

        ((TestRateLimiter)_rateLimiterMock).ReleaseCalledOnce();

    }
}
