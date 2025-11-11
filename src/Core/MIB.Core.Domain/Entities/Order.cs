// Order.cs
using MIB.Core.Domain;
using MIB.Core.Domain.Entities;

namespace MIB.Core.Domain.Entities
{
    /// <summary>
    /// Mappa la tabella dbo.Ordini
    /// </summary>
    public class Order : Entity<int>, IAggregateRoot // id identity
    {
        public virtual bool Confirmed { get; set; }  // Confermato

        // Navigazione
        public virtual Product Product { get; set; } = default!;
    }
}
