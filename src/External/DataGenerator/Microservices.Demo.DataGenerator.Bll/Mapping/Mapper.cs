using Microservices.Demo.DataGenerator.Bll.Models;
using Microservices.Demo.DataGenerator.Messages;
using Item = Microservices.Demo.DataGenerator.Bll.Models.Item;
using MessageItem = Microservices.Demo.DataGenerator.Messages.Item;

namespace Microservices.Demo.DataGenerator.Bll.Mapping;

public static class Mapper
{
    public static OrderInputMessage Map(this Order order)
    {
        return new OrderInputMessage
        {
            RegionId = order.RegionId,
            CustomerId = order.CustomerId,
            Comment = order.Comment,
            Items = { order.Items.Map() }
        };
    }

    private static IReadOnlyList<MessageItem> Map(this IReadOnlyList<Item> items)
    {
        return items
            .Select(item => new MessageItem
            {
                Barcode = item.Barcode,
                Quantity = item.Quantity,
            }).ToArray();
    }
}
