namespace UCommerce.Kentico.Infrastructure
{
    public class KenticoServiceProvider : IKenticoServiceProvider
    {
        public T Resolve<T>() where T : class
        {
            return CMS.Core.Service.Entry<T>();
        }
    }
}
