using Microservices.Demo.OrderService.Bll.Models;
using Microservices.Demo.OrderService.Kafka.Messages;
using BllInputOrder = Microservices.Demo.OrderService.Bll.Models.InputOrder;
using InputOrder = Microservices.Demo.OrderService.Kafka.Messages.InputOrder;

namespace Microservices.Demo.OrderService.Kafka.Extensions;

public static class KafkaExtensions
{
    public static OrderInputErrorsMessage ToInputErrorMessage(
        this BllInputOrder inputOrder,
        string reason,
        string description)
    {
        return new OrderInputErrorsMessage
        {
            InputOrder = new InputOrder
            {
                RegionId = inputOrder.RegionId,
                CustomerId = inputOrder.CustomerId,
                Comment = inputOrder.Comment,
                Items = inputOrder.Items.Select(
                        item => new InputOrderItem
                        {
                            Barcode = item.Barcode,
                            Quantity = item.Quantity
                        })
                    .ToArray()
            },
            ErrorReason = new ErrorReason
            {
                Code = reason,
                Text = description
            }
        };
    }

    public static BllInputOrder ToBll(this InputOrderMessage message)
    {
        return new BllInputOrder(
            RegionId: message.RegionId,
            CustomerId: message.CustomerId,
            Comment: message.Comment,
            Items: message.Items
                .Select(item => new Item(Barcode: item.Barcode, Quantity: item.Quantity))
                .ToArray());
    }
}