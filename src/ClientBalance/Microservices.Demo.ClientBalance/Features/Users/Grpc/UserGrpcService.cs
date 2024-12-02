using Grpc.Core;
using MediatR;
using Microservices.Demo.ClientBalance.Bll.Users.CreateUser;
using Microservices.Demo.ClientBalance.Bll.Users.GetBalance;
using Microservices.Demo.ClientBalance.Features.Users.Grpc.Mappers;
using Microservices.Demo.ClientBalance.Protos;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.Features.Users.Grpc;

public class UserGrpcService : UserServiceGrpc.UserServiceGrpcBase
{
    private readonly ISender _sender;

    public UserGrpcService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<V1CreateUserResponse> V1CreateUser(
        V1CreateUserRequest request,
        ServerCallContext context)
    {
        var createUserCommand = new CreateUserCommand(request.UserId);

        await _sender.Send(createUserCommand, context.CancellationToken);

        return new V1CreateUserResponse();
    }

    public override async Task<V1GetBalanceResponse> V1GetBalance(
        V1GetBalanceRequest request,
        ServerCallContext context)
    {
        var getBalanceQuery = new GetBalanceQuery(request.UserId);

        var balance = await _sender.Send(getBalanceQuery, context.CancellationToken);

        return new V1GetBalanceResponse()
        {
            Balance = balance.ToProtoMoney()
        };
    }
}
