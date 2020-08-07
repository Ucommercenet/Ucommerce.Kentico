using System;
using System.Collections.Generic;
using System.Globalization;
using CMS.Membership;
using System.Linq;
using CMS.Base;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Components.Windsor;
using UCommerce.Security;

namespace UCommerce.Kentico.Security
{
    /// <summary>
    /// Kentico implementation of <see cref="IUserService"/>.
    /// </summary>
    public class UserService : IUserService
    {
        [Mandatory]
        public ICurrentUserNameService CurrentUserNameService { get; set; }
        
        private static readonly object Lock = new object();

        private readonly IUserGroupService _userGroupService;

        /// <summary>
        /// Per web request cached current user.
        /// </summary>
        protected User CurrentUser { get; set; }

        /// <summary>
        /// Per web request cached list of all users.
        /// </summary>
        protected IList<User> AllUsers { get; set; }


        public  UserService(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        public virtual User GetCurrentUser()
        {
            if (CurrentUser == null)
            {
                var username = CurrentUserNameService.CurrentUserName;
                CurrentUser = GetUser(username);
            }

            return CurrentUser;
        }

        public virtual User GetUser(string userName)
        {
            UserInfo kenticoUser = FindKenticoUserFromUsername(userName);

            if (kenticoUser == null)
            {
                throw new ArgumentException(string.Format("Could not find a Kentico UserInfo object, matching the username: '{0}'", userName));
            }

            return GetOrCreateUser(kenticoUser);
        }

        public virtual IList<User> GetAllUsers()
        {
            if (AllUsers == null)
            {
                AllUsers = UserInfoProvider.GetUsers()
                    .WhereNotEquals("UserName", "public")
                    .Select(x => GetOrCreateUser(x)).ToList();
            }

            return AllUsers;
        }

        public virtual CultureInfo GetCurrentUserCulture()
        {
            var user = MembershipContext.AuthenticatedUser;

            if (user == null) throw new InvalidOperationException("MembershipContext.AuthenticatedUser returned null.");

            return CultureInfo.GetCultureInfo(user.PreferredUICultureCode);
        }

        protected virtual User GetOrCreateUser(UserInfo kenticoUser)
        {
            lock (Lock)
            {
                var user = User.SingleOrDefault(u => u.ExternalId.ToUpper() == kenticoUser.UserName.ToUpper());

                if (user == null)
                {
                    user = new User(kenticoUser.UserName) { ExternalId = kenticoUser.UserName };

                    user.Save();
                }   

                // Read the current user groups, for the current user, and setup the UserGroups currently matched to that user.
                // We map Kentico Roles, to uCommerce UserGroups.
                // This mapping is done everytime a user is retrieved.
                ICollection<UserGroup> collectionOfUserGroups = new List<UserGroup>();
                var userRoleIDs = UserRoleInfoProvider.GetUserRoles().Column("RoleID").WhereEquals("UserID", kenticoUser.UserID);
                var roles = RoleInfoProvider.GetRoles().WhereIn("RoleID", userRoleIDs);
                foreach (var role in roles)
                {
                    var userGroup = _userGroupService.GetUserGroup(role.RoleName);
                    if (userGroup != null)
                    {
                        collectionOfUserGroups.Add(userGroup);
                    }
                }

                user.UserGroups = collectionOfUserGroups;

                // Set non-persisted properties.
                user.Name = kenticoUser.FullName;
                user.FirstName = kenticoUser.FirstName;
                user.IsAdmin = kenticoUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin);

                return user;
            }

        }

        /// <summary>
        /// Queries Kentico for the <see cref="UserInfo"/> object mathcing the user name.
        /// </summary>
        /// <param name="userName">The UserName value.</param>
        /// <returns>The UserInfo object if found, null otherwise.</returns>
        protected virtual UserInfo FindKenticoUserFromUsername(string userName)
        {
            var user = UserInfoProvider.GetUsers().WhereEquals("UserName", userName).FirstOrDefault();

            return user;
        }

    }
}
