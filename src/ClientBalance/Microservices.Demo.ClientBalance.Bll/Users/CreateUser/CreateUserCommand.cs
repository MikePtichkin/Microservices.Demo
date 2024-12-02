using MediatR;

namespace Microservices.Demo.ClientBalance.Bll.Users.CreateUser;

public record CreateUserCommand(long UserId) : IRequest;
