using System;

namespace Microservices.Demo.ClientOrders.Domain.Orders;

public static class OrderIdParser
{
    public static string GenerateComment(long id, string guid)
    {
        return $"clients-order-service-id;{id};{guid}";
    }

    public static bool TryParseOrderIdFromComment(string comment, out long id, out Guid guid)
    {
        var parts = comment.Split(';');
        if (parts.Length == 3 &&
            long.TryParse(parts[1], out id) &&
            Guid.TryParse(parts[2], out guid))
        {
            return true;
        }

        id = default;
        guid = default;
        return false;
    }
}
