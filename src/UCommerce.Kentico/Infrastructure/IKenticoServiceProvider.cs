using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCommerce.Kentico.Infrastructure
{
    public interface IKenticoServiceProvider
    {
        T Resolve<T>() where T : class; 
    }
}
