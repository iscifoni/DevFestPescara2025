using FluentNHibernate.Mapping;
using MIB.Core.Domain.Entities;

namespace MIB.Core.Infrastructure.NHibernate.Mapping
{
    public class ProductMapping : ClassMap<Product>
    {
        public ProductMapping()
        {
            Table("Prodotti");

            Id(x => x.Id)
                .Column("id")
                .GeneratedBy.Identity();

            Map(x => x.Description)
                .Column("Descrizione")
                .Nullable();

            Map(x => x.Price)
                .Column("Prezzo")
                .Nullable();

            HasMany(x => x.Inventories)
                .Table("Magazzino")
                .KeyColumn("idProdotto")
                .Inverse()
                .Cascade.None();

            HasMany(x => x.Orders)
                .Table("Ordini")
                .KeyColumn("idProdotto")
                .Inverse()
                .Cascade.None();
        }
    }
}
