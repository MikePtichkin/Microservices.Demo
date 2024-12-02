namespace Microservices.Demo.ClientBalance.Features.Users.Grpc.Mappers;

public static class MoneyMapper
{
    public static Google.Type.Money ToProtoMoney(this decimal amount)
    {
        var units = (long)amount;
        var nanos = (int)((amount - units) * 1_000_000_000);

        return new Google.Type.Money
        {
            Units = units,
            Nanos = nanos
        };
    }
}
