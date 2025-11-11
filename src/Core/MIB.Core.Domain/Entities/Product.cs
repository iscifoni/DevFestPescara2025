// Product.cs
using MIB.Core.Domain;
using System.Collections.Generic;

namespace MIB.Core.Domain.Entities
{
    public class Product : Entity<int>, IAggregateRoot
    {
        public virtual string? Description { get; set; }   // Descrizione
        public virtual string? Price { get; set; }         // Prezzo (nel DB è nchar(10))

        // Navigazioni (opzionali ma utili)
        public virtual IList<InventoryItem> Inventories { get; set; } = new List<InventoryItem>();
        public virtual IList<Order> Orders { get; set; } = new List<Order>();
    }
}
