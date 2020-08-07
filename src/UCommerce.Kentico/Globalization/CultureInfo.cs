using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCommerce.Kentico.Globalization
{
    /// <summary>
    /// An abstraction to maintain compatibility between Kentico 11 and 12. CultureInfo moved assembly in major version 
    /// So we need a general abstraction that contains the properties that we need between the two. 
    /// </summary>
    public class CultureInfo
    {
        public string CultureName { get; set; }

        public string CultureCode { get; set; }

        public string CultureAlias { get; set; }
    }
}
