using System.Threading;

namespace Microservices.Demo.ViewOrder.Infra.UnitTests;

public class BaseTests
{
    protected CancellationToken Token = CancellationToken.None;
}