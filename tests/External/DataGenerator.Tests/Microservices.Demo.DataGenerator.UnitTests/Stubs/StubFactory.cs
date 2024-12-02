namespace Microservices.Demo.DataGenerator.UnitTests.Stubs;

public static class StubFactory
{
    public static GenerateOrdersHandlerStub CreateGenerateOrdersHandlerStub()
        => new(new(), new(), new(), new(), new());
}
