using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.EmailEngine;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Definitions;
using UCommerce.Infrastructure.Components.Windsor;
using UCommerce.Presentation.Web.Controls;
using UCommerce.Security;

namespace UCommerce.Kentico.UI.Controls
{
    public class EmailControlFactory : IControlFactory
    {
        [Mandatory]
        public ITextSanitizer TextSanitizer { get; set; }

        private readonly IDataTypeDefinitionInspector _dataTypeDefinitionInspector;
        private DataTypeDefinition _contentDataTypeDefinition = new EmailContentDataTypeDefinition();

        public EmailControlFactory(IDataTypeDefinitionInspector dataTypeDefinitionInspector)
        {
            _dataTypeDefinitionInspector = dataTypeDefinitionInspector;
        }

        public bool Supports(DataType dataType)
        {
            var dataTypeDefinitionName = _dataTypeDefinitionInspector.GetDataTypeDefintion(dataType);

            return dataTypeDefinitionName.Equals(_contentDataTypeDefinition.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public Control GetControl(IProperty property)
        {
            var safeDropDownList = new SafeDropDownList();
            safeDropDownList.ID = "MediaContentPicker";
            safeDropDownList.EnableViewState = true;

            foreach (EmailTemplateInfo emailTemplateInfo in CMS.EmailEngine.EmailTemplateProvider.GetEmailTemplates())
            {
                var listItem = new ListItem(
                    TextSanitizer.SanitizeOutput(emailTemplateInfo.TemplateDisplayName),
                    TextSanitizer.SanitizeOutput(emailTemplateInfo.TemplateID.ToString())
                );
                listItem.Selected = property.GetValue() != null && emailTemplateInfo.TemplateID.ToString() == property.GetValue().ToString();

                safeDropDownList.Items.Add(listItem);
            }

            return safeDropDownList;
        }
    }
}
