namespace Microservices.Demo.ClientBalance.Features.Operations.Grpc.Mappers;

public static class MoneyMapper
{
    private const long NanosPerUnit = 1_000_000_000;

    public static Google.Type.Money ToProtoMoney(this decimal amount)
    {
        var units = (long)amount;
        var nanos = (int)((amount - units) * NanosPerUnit);

        return new Google.Type.Money
        {
            Units = units,
            Nanos = nanos
        };
    }

    public static decimal ToDecimal(this Google.Type.Money money) =>
        money.Units + money.Nanos / NanosPerUnit;
}
