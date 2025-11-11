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
    [OutboxEvent(PubSubName = "pubsubamq", TopicName = "OrderDeletedEventHandler")]
    public class OrderDeletedEventHandler(IRepository<InventoryItem> inventoryRepository) : IOutboxDomainEventHandler<EntityDeletedEvent<Order>>
    //public class OrderDeletedEventHandler(IRepository<InventoryItem> inventoryRepository) : IInboxDomainEventHandler<EntityDeletedEvent<Order>>

    {
        public async Task Handle(EntityDeletedEvent<Order> notification, CancellationToken cancellationToken)
        {
            var item = inventoryRepository.DataSet.Where(a => a.Product.Id == notification.Entity.Product.Id).First();

            if (item.Quantity > 1)
            {
                item.Quantity++;
                await inventoryRepository.UpdateAsync(item);
            }
        }
    }
}
