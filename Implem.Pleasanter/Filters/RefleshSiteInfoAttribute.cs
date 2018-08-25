using Implem.Pleasanter.Libraries.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class RefleshSiteInfoAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            SiteInfo.Reflesh();
        }
    }
}