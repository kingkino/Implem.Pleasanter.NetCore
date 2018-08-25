using System.Web;
using AspNetCoreCurrentRequestContext;
using Implem.Pleasanter.Libraries.Server;

namespace Implem.Pleasanter.Libraries.Requests
{
    public static class ViewModes
    {
        public static string GetBySession(long siteId)
        {
            return Sessions.Get<string>("ViewMode" + siteId) ?? "index";
        }

        public static void Set(long siteId)
        {
            Sessions.Set("ViewMode" + siteId, Routes.Action().ToLower());
        }
    }
}