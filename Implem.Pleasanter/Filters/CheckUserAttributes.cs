using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.DataSources;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Security;
using Implem.Pleasanter.Libraries.Server;
using Implem.Pleasanter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class CheckUserAttributes : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (Sessions.LoggedIn())
            {
                var userModel = new UserModel().Get(
                    ss: null,
                    where: Rds.UsersWhere()
                        .TenantId(Sessions.TenantId())
                        .UserId(Sessions.UserId())
                        .Disabled(0));
                if (userModel.AccessStatus != Databases.AccessStatuses.Selected)
                {
                    Authentications.SignOut();
                    filterContext.Result = new RedirectResult(Locations.Login());
                }
                else
                {
                    userModel.SetSession();
                }
            }
        }
    }
}