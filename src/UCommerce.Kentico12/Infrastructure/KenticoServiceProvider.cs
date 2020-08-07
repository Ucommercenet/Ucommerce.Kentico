using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCommerce.Kentico.Infrastructure;

namespace UCommerce.Kentico12.Infrastructure
{
    public class KenticoServiceProvider : IKenticoServiceProvider
    {
        public T Resolve<T>() where T : class
        {
            return CMS.Core.Service.Resolve<T>();
        }
    }
}
