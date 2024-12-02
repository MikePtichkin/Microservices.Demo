namespace Microservices.Demo.ReportService.Infra.Options;

public class CsvReportGeneratorOptions
{
    public int CsvLineProcessingDelayMs { get; init; }
    public int MaxConcurrentRequests { get; init; }
}