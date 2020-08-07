using CMS.DataEngine;
using CMS.Membership;
using CMS.SiteProvider;
using System.Linq;
using System.Web;
using IAuthenticationService = UCommerce.Security.IAuthenticationService;

namespace UCommerce.Kentico.Security
{
    /// <summary>
    /// Kentico implementation of <see cref="UCommerce.Security.IAuthenticationService"/>.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        public virtual bool IsAuthenticated()
        {
            CurrentUserInfo currentUser = MembershipContext.AuthenticatedUser;

            if (currentUser == null) return false;

            if (!currentUser.Enabled) return false;

            if (currentUser.IsPublic()) return false;

            // CurrentUserInfo.IsAuthorizedPerResource makes a check for 
            // UserPrivilegeLevelEnum.GlobalAdmin internally, thus allowing admins access to
            // all resources, including UCommerce.
            // Non global-admins need to have just the READ roles set for "Ucommerce".
            // We can't check for "Modify" here because it causes a redirect loop when the user has
            // just the read permission.
            if (!currentUser.IsAuthorizedPerResource("Ucommerce", "Read")) return false;

            return true;
        }

        public virtual void PromptForLogin()
        {
            string logonPageUrl = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSSecuredAreasLogonPage");
            var context = HttpContext.Current;
            context.Response.Redirect(logonPageUrl);
        }
    }
}
