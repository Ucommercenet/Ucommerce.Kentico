using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCommerce.Kentico.Globalization
{
    /// <summary>
    /// An abstraction to maintain compatibility between Kentico 11 and 12. LocalizationContext moved assembly in major version 
    /// So we need a general abstraction that contains the properties that we need between the two. 
    /// </summary>
    public interface IKenticoLocalizationContext
    {
        string PreferredCultureCode { get; }
    }

    public interface IKenticoCultureInfoProvider
    {
        CultureInfo GetCultureInfoForCulture(string currentDocumentCulture);
    }
}
