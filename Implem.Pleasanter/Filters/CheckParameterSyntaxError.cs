using Implem.DefinitionAccessor;
using Implem.Pleasanter.Libraries.Requests;
using Implem.Pleasanter.Libraries.Responses;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class CheckParameterSyntaxError : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (Routes.Controller() != "errors" &&  Parameters.SyntaxErrors?.Any() == true)
            {
                filterContext.Result = new RedirectResult(Locations.ParameterSyntaxError());
            }
        }
    }
}