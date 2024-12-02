using Grpc.Core;
using MediatR;
using Microservices.Demo.ClientBalance.Bll.Balances.WithdrawBalance;
using Microservices.Demo.ClientBalance.Bll.Operations.ChangeOperationStatus;
using Microservices.Demo.ClientBalance.Bll.Operations.TopUpBalance;
using Microservices.Demo.ClientBalance.Features.Operations.Grpc.Mappers;
using Microservices.Demo.ClientBalance.Protos;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Features.Operations.Grpc;

public class OperationGrpcService : OperationServiceGrpc.OperationServiceGrpcBase
{
    private readonly ISender _sender;

    public OperationGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<V1TopUpBalanceResponse> V1TopUpBalance(
        V1TopUpBalanceRequest request,
        ServerCallContext context)
    {
        var topUpBalanceCommand = new TopUpBalanceCommand(
            OperationId: Guid.Parse(request.OperationId),
            UserId: request.UserId,
            Amount: request.Amount.ToDecimal(),
            CreatedAt: request.OccuredAt.ToDateTimeOffset());

        await _sender.Send(topUpBalanceCommand, context.CancellationToken);

        return new V1TopUpBalanceResponse();
    }

    public override async Task<V1WithdrawBalanceResponse> V1WithdrawBalance(
        V1WithdrawBalanceRequest request,
        ServerCallContext context)
    {
        var withdrawBalanceCommand = new WithdrawBalanceCommand(
            OperationId: Guid.Parse(request.OperationId),
            UserId: request.UserId,
            Amount: request.Amount.ToDecimal(),
            CreatedAt: request.OccuredAt.ToDateTimeOffset());

        var result = await _sender.Send(withdrawBalanceCommand, context.CancellationToken);

        return new V1WithdrawBalanceResponse
        {
            Success = result,
        };
    }

    public async override Task<V1ChangeOperationStatusResponse> V1ChangeOperationStatus(
        V1ChangeOperationStatusRequest request,
        ServerCallContext context)
    {
        var changeOperationStatusCommand = new ChangeOperationStatusCommand(
            OperationId: Guid.Parse(request.OperationId),
            Type: request.Type.ToDomainType(),
            UserId: request.UserId,
            Status: request.Status.ToDomainStatus(),
            UpdatedAt: request.OccuredAt.ToDateTimeOffset());

        await _sender.Send(
            changeOperationStatusCommand,
            context.CancellationToken);

        return new V1ChangeOperationStatusResponse();
    }
}
