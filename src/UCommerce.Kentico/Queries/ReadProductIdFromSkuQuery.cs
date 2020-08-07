using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using UCommerce.EntitiesV2.Queries;

namespace UCommerce.Kentico.Queries
{
    public class ReadProductIdFromSkuQuery : ICannedQuery<ProductIdAndVariantIdView>
    {
        private readonly string _sku;

        public ReadProductIdFromSkuQuery(string sku)
        {
            _sku = sku;
        }

        public IEnumerable<ProductIdAndVariantIdView> Execute(ISession session)
        {
            string sql =
                @"select
                    product.ProductId AS ProductId,
					0 AS VariantId
    		    from uCommerce_Product product
                where product.Sku = :sku
                    and product.VariantSku is NULL";

            var query = session.CreateSQLQuery(sql)
                .SetString("sku", _sku);

            return query
                .SetResultTransformer(Transformers.AliasToBean(typeof(ProductIdAndVariantIdView)))
                .List<ProductIdAndVariantIdView>();
        }
    }
}
