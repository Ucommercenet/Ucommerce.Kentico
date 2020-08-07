using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using UCommerce.Documents;
using UCommerce.Infrastructure;
using UCommerce.Search;
using UCommerce.Security;

namespace UCommerce.Kentico.CMSFormControls.Selectors
{
    public partial class ProductIdSelector : FormEngineUserControl
    {
        protected ITextSanitizer TextSanitizer
        {
            get { return ObjectFactory.Instance.Resolve<ITextSanitizer>(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string SelectableType
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SelectableType"), "sellableproduct");
            }
            set
            {
                SetValue("SelectorWidth", value);
            }
        }

        public override object Value
        {
            get
            {
                return SearchResult.SelectedValue;
            }
            set
            {
                // Sets the selected list item the ListBox.
                var selectedItem = GetProductNameForSelectedType((int) value);
                var listItem = new ListItem(selectedItem, value.ToString()){Selected = true};
                SearchResult.Items.Add(listItem);
            }
        }

        protected virtual string GetProductNameForSelectedType(int productId)
        {
            var product = GetProductForTheType(productId);

            return product.Text;
        }

        protected virtual ListItem GetProductForTheType(int productId)
        {
            var index = ObjectFactory.Instance.Resolve<IGetFullTextSearchIndex>().GetFullTextSearchIndex();
                
            return index.Where(x => x.ItemId == productId &&  x.ItemTypes.Contains(SelectableType, StringComparer.CurrentCultureIgnoreCase)).Select(x => new ListItem(TextSanitizer.SanitizeOutput(x.Name), x.ItemId.ToString())).FirstOrDefault();
            
        }

        protected virtual void SearchForProduct_OnClick(object sender, EventArgs e)
        {
            SearchResult.Items.Clear();

            var fullTextSearchByName = ObjectFactory.Instance.Resolve<IFullTextSearchByName>();
            var searchResult = fullTextSearchByName.SearchByName(SearchTerm.Text);
            var filteredResult = FilterSearchResultToMatchSpecifiedType(searchResult);

            if (filteredResult.Any())
            {
                SearchResult.Items.AddRange(filteredResult);
            }
        }

        protected virtual ListItem[] FilterSearchResultToMatchSpecifiedType(IEnumerable<SearchRecord> records)
        {
            return records.Where(x => x.ItemTypes.Contains(SelectableType, StringComparer.CurrentCultureIgnoreCase)).Select(x => new ListItem(TextSanitizer.SanitizeOutput(x.Name), x.ItemId.ToString())).ToArray();
        }
    }
}