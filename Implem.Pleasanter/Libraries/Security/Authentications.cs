using AspNetCoreCurrentRequestContext;
using Implem.Pleasanter.Libraries.Settings;
using Implem.Pleasanter.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Web;
namespace Implem.Pleasanter.Libraries.Security
{
    public static class Authentications
    {
        public static string SignIn(string returnUrl)
        {
            return new UserModel(
                SiteSettingsUtilities.UsersSiteSettings(),
                setByForm: true)
                    .Authenticate(returnUrl);
        }

        public static bool Try(string loginId, string password)
        {
            return new UserModel(SiteSettingsUtilities.UsersSiteSettings(), setByForm: true)
                .Authenticate();
        }

        public static void SignOut()
        {
            AspNetCoreHttpContext.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            AspNetCoreHttpContext.Current.Session.Clear();
        }

        public static bool Windows()
        {
            return false;
            //TODO Windows Authentication
            //return ((AuthenticationSection)ConfigurationManager
            //    .GetSection("system.web/authentication")).Mode.ToString() == "Windows";
        }
    }
}