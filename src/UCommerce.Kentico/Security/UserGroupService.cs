using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Membership;
using UCommerce.EntitiesV2;
using UCommerce.Security;

namespace UCommerce.Kentico.Security
{
    /// <summary>
    /// Kentico implementation of <see cref="IUserGroupService"/>.
    /// </summary>
    public class UserGroupService : IUserGroupService
    {
        private static readonly object Lock = new object();

        /// <summary>
        /// Per web request cached list of UserGroups.
        /// </summary>
        protected virtual IList<UserGroup> AllUserGroups { get; set; }

        public virtual IList<UserGroup> GetAllUserGroups()
        {
            if (AllUserGroups == null)
            {
                var roles = RoleInfoProvider.GetAllRoles(0, false, false).ToList().GroupBy(x => x.RoleName).Select(x => x.First());
                AllUserGroups = roles.Select(x => GetOrCreateUserGroup(x)).ToList();
            }

            return AllUserGroups;
        }

        public virtual UserGroup GetUserGroup(string externalId)
        {
            RoleInfo role = RoleInfoProvider.GetRoles().WhereEquals("RoleName", externalId).FirstOrDefault();

            if (role == null)
            {
                return null;
            }

            return GetOrCreateUserGroup(role);
        }

        protected virtual UserGroup GetOrCreateUserGroup(RoleInfo roleInfo)
        {
            lock (Lock)
            {
                var userGroup = UserGroup.SingleOrDefault(u => u.ExternalId.ToUpper() == roleInfo.RoleName.ToUpper());

                if (userGroup == null)
                {
                    userGroup = new UserGroup()
                    {
                        ExternalId = roleInfo.RoleName,
                        Name = roleInfo.RoleDisplayName
                    };

                    userGroup.Save();
                }

                userGroup.Name = roleInfo.RoleDisplayName;

                return userGroup;
            }
        }
    }
}
