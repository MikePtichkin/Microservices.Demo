using MediatR;

namespace Microservices.Demo.ReportService.Bll.Reports.GetCsvCustomerOrdersReport;

public sealed record GetCsvCustomerOrdersReportQuery(
    long CustomerId) : IRequest<byte[]>;