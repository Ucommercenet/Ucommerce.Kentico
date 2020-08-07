using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCommerce.Kentico.Ems
{
    /// <summary>
    /// Interface for returning the current order guid.
    /// </summary>
    public interface IProvideCurrentOrderGuid
    {
        string GetCurrentOrderGuid();
    }
}
