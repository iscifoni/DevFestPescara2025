using FluentNHibernate.Mapping;
using MIB.Core.Domain.Entities;

namespace MIB.Core.Infrastructure.NHibernate.Mapping
{
    public class OrderMapping : ClassMap<Order>
    {
        public OrderMapping()
        {
            Table("Ordini");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();


            Map(x => x.Confirmed)
                .Column("Confermato")
                .Not.Nullable();

            References(x => x.Product)
                .Class<Product>()
                .Column("idProdotto")
                .Not.Nullable()
                .ForeignKey("FK_Ordini_Prodotti")
                .Not.LazyLoad();
        }
    }
}
