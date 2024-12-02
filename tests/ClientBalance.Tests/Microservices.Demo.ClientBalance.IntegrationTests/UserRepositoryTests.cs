using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microservices.Demo.ClientBalance.Domain.Users;
using Microservices.Demo.ClientBalance.Infra.Dal.Common;
using Microservices.Demo.ClientBalance.Infra.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.ClientBalance.IntegrationTests;

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly UserRepository _userRepository;
    private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;

    public UserRepositoryTests(DatabaseFixture fixture)
    {
        _connectionFactory = fixture.ServiceProvider.GetRequiredService<IDbConnectionFactory<NpgsqlConnection>>();
        var logger = new Logger<UserRepository>(new LoggerFactory());
        _userRepository = new UserRepository(_connectionFactory, logger);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUserId()
    {
        // Arrange
        var random = new Random();
        int randomId = random.Next(1, 1000000);
        var user = new User(id: randomId, balance: 100);

        // Act
        var result = await _userRepository.Create(user, CancellationToken.None);

        // Assert
        Assert.Equal(randomId, result);
    }
}