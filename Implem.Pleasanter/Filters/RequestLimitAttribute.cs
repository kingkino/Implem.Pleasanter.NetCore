using Implem.DefinitionAccessor;
using Implem.Libraries.Classes;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Security;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AspNetCoreCurrentRequestContext;

namespace Implem.Pleasanter.Filters
{
    public class RequestLimitAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Parameters.Security.RequestLimit > 0)
            {
                var userHostAddress = AspNetCoreHttpContext.Current?.Connection.RemoteIpAddress?.ToString();
                var currentExecutionFilePath = AspNetCoreHttpContext.Current.Request.Path;
                var errorUrl = Locations.BadRequest();
                if (userHostAddress != null && currentExecutionFilePath != errorUrl)
                {
                    var thisMinute = DateTime.Now.ToString("t");
                    if (!Defenses.RequestVolume.ContainsKey(userHostAddress))
                    {
                        try
                        {
                            Defenses.RequestVolume.Add(
                                userHostAddress, new TwoData<string, int>(thisMinute, 0));
                            Defenses.RequestVolume.RemoveAll((k, v) => v.Data1 != thisMinute);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (Defenses.RequestVolume[userHostAddress].Data1 == thisMinute)
                    {
                        Defenses.RequestVolume[userHostAddress].Data2++;
                        if (Defenses.RequestVolume[userHostAddress].Data2 >
                            Parameters.Security.RequestLimit)
                        {
                            filterContext.Result = new RedirectResult(errorUrl);
                            base.OnActionExecuting(filterContext);
                        }
                    }
                    else
                    {
                        Defenses.RequestVolume[userHostAddress].Data1 = thisMinute;
                        Defenses.RequestVolume[userHostAddress].Data2 = 1;
                    }
                }
            }
        }
    }
}