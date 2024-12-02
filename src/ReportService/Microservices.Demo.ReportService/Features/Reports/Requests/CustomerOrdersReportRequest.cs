using Microsoft.AspNetCore.Mvc;

namespace Microservices.Demo.ReportService.Features.Reports.Requests;

public sealed record CustomerOrdersReportRequest
{
    [FromRoute(Name = "customerId")]
    public long CustomerId { get; init; }
}
