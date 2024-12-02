namespace Microservices.Demo.ClientBalance.Domain.Operations;

public enum OperationStatus
{
    Unspecified = 0,
    Pending = 1,
    Cancelled = 2,
    Completed = 3,
    Rejected = 4
}
