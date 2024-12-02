using FluentValidation;
using Microservices.Demo.ReportService.Features.Reports.Requests;

namespace Microservices.Demo.ReportService.Features.Reports.Validators;

public class GetCustomerOrdersReportValidator : AbstractValidator<CustomerOrdersReportRequest>
{
    public GetCustomerOrdersReportValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
    }
}
