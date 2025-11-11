using MIB.Core.Domain;
using MIB.Core.Domain.Entities;
using MIB.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIB.Microservices.OrderService.Application.Event
{
    [OutboxEvent(PubSubName = "pubsubamq", TopicName = "OrderCreatedEventHandler")]
    public class OrderCreatedEventHandler(IRepository<InventoryItem> inventoryRepository, IRepository<Order> orderRepository) : IOutboxDomainEventHandler<EntityCreatedEvent<Order>>
    //public class OrderCreatedEventHandler(IRepository<InventoryItem> inventoryRepository, IRepository<Order> orderRepository) : IInboxDomainEventHandler<EntityCreatedEvent<Order>>
    {

        public async Task Handle(EntityCreatedEvent<Order> notification, CancellationToken cancellationToken)
        {
            var item = inventoryRepository.DataSet.Where(a=> a.Product.Id == notification.Entity.Product.Id).First();

            if (item.Quantity > 1)
            {
                item.Quantity--;
                await inventoryRepository.UpdateAsync(item);

                notification.Entity.Confirmed = true;
                await orderRepository.UpdateAsync(notification.Entity);
            }
        }
    }
}
