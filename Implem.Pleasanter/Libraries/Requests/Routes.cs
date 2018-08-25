using AspNetCoreCurrentRequestContext;
using Implem.Libraries.Utilities;
using System.Linq;
using System.Web;
namespace Implem.Pleasanter.Libraries.Requests
{
    public static class Routes
    {
        public static string Controller()
        {
            if (AspNetCoreHttpContext.Current != null)
            {
                var controller = Url.RouteData("controller").ToLower();
                if (!string.IsNullOrEmpty(controller)) return controller;
            }
            return StackTraces.Class();
        }

        public static string Action()
        {
            if (AspNetCoreHttpContext.Current != null)
            {
                var action = Url.RouteData("action").ToLower();
                if (!string.IsNullOrEmpty(action)) return action;
            }
            return StackTraces.Method();
        }

        public static long Id()
        {
            return Url.RouteData("id").ToLong();
        }
    }
}