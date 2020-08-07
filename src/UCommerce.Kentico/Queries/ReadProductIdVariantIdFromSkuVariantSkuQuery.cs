using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using UCommerce.EntitiesV2.Queries;

namespace UCommerce.Kentico.Queries
{
    public class ReadProductIdVariantIdFromSkuVariantSkuQuery : ICannedQuery<ProductIdAndVariantIdView>
    {
        private readonly string _sku;
        private readonly string _variantSku;

        public ReadProductIdVariantIdFromSkuVariantSkuQuery(string sku, string variantSku)
        {
            _sku = sku;
            _variantSku = variantSku;
        }

        public IEnumerable<ProductIdAndVariantIdView> Execute(ISession session)
        {
            string sql =
                @"select
                    product.ParentProductId AS ProductId,
					product.ProductId AS VariantId
    		    from uCommerce_Product product
                where product.Sku = :sku
                    and product.VariantSku = :variantSku";

            var query = session.CreateSQLQuery(sql)
                .SetString("sku", _sku)
                .SetString("variantSku", _variantSku);

            return query
                .SetResultTransformer(Transformers.AliasToBean(typeof(ProductIdAndVariantIdView)))
                .List<ProductIdAndVariantIdView>();
        }
    }
}
