using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservices.Demo.ReportService.Bll.Reports.GetCsvCustomerOrdersReport;
using Microservices.Demo.ReportService.Features.Reports.Requests;
using Microservices.Demo.ReportService.Infrastructure.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ReportService.Features.Reports.Controllers;

[ApiController]
[GlobalExceptionFilter]
[Route("v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ReportsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<FileContentResult> GetCustomerOrdersReport(
        [FromQuery] CustomerOrdersReportRequest request,
        CancellationToken cancellationToken)
    {
        var getCustomerOrdersReportQuery = new GetCsvCustomerOrdersReportQuery(request.CustomerId);
        var csvBytes = await _sender.Send(getCustomerOrdersReportQuery, cancellationToken);

        var fileName = $"Customer_{request.CustomerId}_OrdersReport.csv";
        return File(csvBytes, "text/csv", fileName);
    }
}
