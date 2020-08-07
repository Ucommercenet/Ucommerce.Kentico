using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace UCommerce.Kentico.Logging
{
    public class LoggerFactory: ILoggerFactory
    {
        public IInternalLogger LoggerFor(string keyName)
        {
            return new Logger();
        }

        public IInternalLogger LoggerFor(Type type)
        {
            return new Logger();
        }
    }
}
