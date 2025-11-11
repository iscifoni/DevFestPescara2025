// InventoryItem.cs
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;

namespace MIB.Core.Domain.Entities
{
    /// <summary>
    /// Mappa la tabella dbo.Magazzino
    /// </summary>
    public class InventoryItem : Entity<int>, IAggregateRoot
    {
        public virtual short Quantity { get; set; }  // Qta

        // Navigazione
        public virtual Product Product { get; set; } = default!;
    }
}
