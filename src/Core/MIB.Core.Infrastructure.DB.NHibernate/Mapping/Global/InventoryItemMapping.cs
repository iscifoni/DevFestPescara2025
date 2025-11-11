using FluentNHibernate.Mapping;
using MIB.Core.Domain.Entities;

namespace MIB.Core.Infrastructure.NHibernate.Mapping
{
    public class InventoryItemMapping : ClassMap<InventoryItem>
    {
        public InventoryItemMapping()
        {
            Table("Magazzino");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();


            Map(x => x.Quantity)
                .Column("Qta")
                .Not.Nullable();

            References(x => x.Product)
                .Column("idProdotto")
                .Not.Nullable()
                .ForeignKey("FK_Prodotti_Magazzino")
                .Not.LazyLoad();
        }
    }
}
