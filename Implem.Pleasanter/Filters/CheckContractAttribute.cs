﻿using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Security;
using Implem.Pleasanter.Libraries.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class CheckContractAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (Sessions.LoggedIn() && Contract.OverDeadline())
            {
                Authentications.SignOut();
                filterContext.Result = new RedirectResult(Locations.Login() + "?expired=1");
            }
        }
    }
}