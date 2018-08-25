using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.DataSources;
using Implem.Pleasanter.Libraries.Requests;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Server;
using Implem.Pleasanter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class CheckApiAuthentication : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var api = Forms.String().Deserialize<Api>();
            if (api?.ApiKey.IsNullOrEmpty() == false)
            {
                var userModel = new UserModel().Get(
                    ss: null,
                    where: Rds.UsersWhere()
                        .ApiKey(api.ApiKey)
                        .Disabled(0));
                if (userModel.AccessStatus != Databases.AccessStatuses.Selected)
                {
                    filterContext.Result = ApiResults.Unauthorized();
                }
                else
                {
                    Sessions.SetTenantId(userModel.TenantId);
                    userModel.SetSession();
                    if (!Contract.Api())
                    {
                        Sessions.Abandon();
                        filterContext.Result = ApiResults.BadRequest();
                    }
                }
            }
            else if (!Sessions.LoggedIn())
            {
                filterContext.Result = ApiResults.Unauthorized();
            }
        }
    }
}