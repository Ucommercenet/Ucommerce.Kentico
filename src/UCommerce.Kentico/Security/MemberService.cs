using System.Collections.Generic;
using System.Linq;
using CMS.Membership;
using UCommerce.EntitiesV2;
using UCommerce.Runtime;
using UCommerce.Security;
using SiteContext = CMS.SiteProvider.SiteContext;

namespace UCommerce.Kentico.Security
{
    /// <summary>
    /// Kentico implementation of <see cref="IMemberService"/>.
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly IOrderContext _orderContext;

        public MemberService(IOrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        /// <summary>
        /// Web request cached list of <see cref="MemberGroup"/>.
        /// </summary>
        protected virtual IList<MemberGroup> MemberGroups { get; set; }
        
        public virtual IList<MemberGroup> GetMemberGroups()
        {
            if (MemberGroups == null)
            {
                var roles = RoleInfoProvider.GetAllRoles(0, false, false).ToList().GroupBy(x => x.RoleName).Select(x => x.First());
                MemberGroups = roles.Select(ConvertKenticoRoleToMemberGroup).ToList();
            }

            return MemberGroups;
        }

        public virtual IList<MemberType> GetMemberTypes()
        {
            return new List<MemberType>();
        }

        public virtual bool IsLoggedIn()
        {
            var user = MembershipContext.AuthenticatedUser;
            return user.UserName != "public";
        }

        public virtual Member GetCurrentMember()
        {
            var user = MembershipContext.AuthenticatedUser;
            if (user.UserName != "public")
            {
                return ConvertKenticoUserToMember(user);
            }
            return null;
        }

        public virtual bool IsMember(string emailAddress)
        {
            var user = UserInfoProvider.GetUsers().WhereEquals("Email", emailAddress).FirstOrDefault();
            return user != null;
        }

        public virtual Member GetMemberFromEmail(string emailAddress)
        {
            var user = UserInfoProvider.GetUsers().WhereEquals("Email", emailAddress).FirstOrDefault();
            if (user != null)
            {
                return ConvertKenticoUserToMember(user);
            }

            return null;
        }

        public virtual Member GetMemberFromLoginName(string loginName)
        {
            var user = UserInfoProvider.GetUsers().WhereEquals("UserName", loginName).FirstOrDefault();
            if (user != null)
            {
                return ConvertKenticoUserToMember(user);
            }

            return null;
        }

        public virtual Member MakeNew(string loginName, string password, string email, MemberType memberType, MemberGroup memberGroup)
        {
            UserInfo user;
            if (_orderContext.HasBasket)
            {
                var currentBillingAddress = _orderContext.GetBasket(false).PurchaseOrder.BillingAddress;
                user = new UserInfo
                {
                    UserName = loginName,
                    FullName = $"{currentBillingAddress.FirstName} {currentBillingAddress.LastName}",
                    FirstName = currentBillingAddress.FirstName,
                    LastName = currentBillingAddress.LastName,
                    Email = email
                };
            }
            else
            {
                user = new UserInfo
                {
                    UserName = loginName,
                    FullName = loginName,
                    Email = email
                };
            }
            
            if (UserInfoProvider.GetUserInfo(loginName) == null)
            {
                UserInfoProvider.SetUserInfo(user);
            }

            // Add user to the currently browsed site
            UserInfoProvider.AddUserToSite(user.UserName, SiteContext.CurrentSiteName);

            // Gets the role
            RoleInfo role = RoleInfoProvider.GetRoleInfo(memberGroup.MemberGroupId, SiteContext.CurrentSiteName);

            if (role != null)
            {
                // Adds the user to the role
                UserInfoProvider.AddUserToRole(user.UserName, role.RoleName, SiteContext.CurrentSiteName);
            }

            return ConvertKenticoUserToMember(user);
        }

        protected virtual MemberGroup ConvertKenticoRoleToMemberGroup(RoleInfo role)
        {
            var memberGroup = new MemberGroup(role.RoleName, role.RoleDisplayName);

            return memberGroup;
        }

        protected virtual Member ConvertKenticoUserToMember(UserInfo user)
        {
            var member = new Member
            {
                LoginName = user.UserName,
                MemberId = user.UserName,
                Email = user.Email
            };

            return member;
        }
    }
}
