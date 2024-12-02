using Microservices.Demo.ClientBalance.Domain.Operations;
using System;

namespace Microservices.Demo.ClientBalance.Features.Operations.Grpc.Mappers;

public static class V1ChangeOperationStatusMapper
{
    public static OperationType ToDomainType(
        this Protos.OperationType grpcType) => grpcType switch
    {
        Protos.OperationType.TopUp => OperationType.TopUp,
        Protos.OperationType.Withdrawal => OperationType.Withdraw,
        _ => throw new ArgumentOutOfRangeException(
            nameof(grpcType),
            $"Unknown operation type: {grpcType}")
    };

    public static OperationStatus ToDomainStatus(
        this Protos.OperationStatus grpcStatus) => grpcStatus switch
    {
        Protos.OperationStatus.Pending => OperationStatus.Pending,
        Protos.OperationStatus.Cancelled => OperationStatus.Cancelled,
        Protos.OperationStatus.Completed => OperationStatus.Completed,
        Protos.OperationStatus.Rejected => OperationStatus.Rejected,
        _ => throw new ArgumentOutOfRangeException(
            nameof(grpcStatus),
            $"Unknown operation status: {grpcStatus}")
    };
    
}
