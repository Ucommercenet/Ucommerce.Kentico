using System;
using CMS.Membership;
using UCommerce.Security;

namespace UCommerce.Kentico.Security
{
    public class CurrentUserNameService : ICurrentUserNameService
    {
        public string CurrentUserName
        {
            get
            {
                var user = MembershipContext.AuthenticatedUser;

                if (user == null)
                    throw new InvalidOperationException("MembershipContext.AuthenticatedUser returned null.");

                return user.UserName;
            }
        }
    }
}