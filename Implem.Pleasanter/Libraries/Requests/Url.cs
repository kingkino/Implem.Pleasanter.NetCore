using System.Web;
using AspNetCoreCurrentRequestContext;
using Microsoft.AspNetCore.Routing;

namespace Implem.Pleasanter.Libraries.Requests
{
    public static class Url
    {
        public static string Server()
        {
            return
                AspNetCoreHttpContext.Current.Request.Scheme + "://" +
                AspNetCoreHttpContext.Current.Request.Host;
        }

        public static string ApplicationPath()
        {
            var path = AspNetCoreHttpContext.Current.Request.PathBase.Value;
            return path.EndsWith("/")
                ? path
                : path + "/";
        }

        public static string AbsoluteUri()
        {
            var request = AspNetCoreHttpContext.Current.Request;
            var uriBuilder = new System.UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if(request.Host.Port.HasValue) uriBuilder.Port = request.Host.Port.Value;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            var uri = uriBuilder.Uri.AbsoluteUri;
            return uri;
        }

        public static string LocalPath()
        {
            var request = AspNetCoreHttpContext.Current.Request;
            var uriBuilder = new System.UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if (request.Host.Port.HasValue) uriBuilder.Port = request.Host.Port.Value;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            var uri = uriBuilder.Uri.LocalPath;
            return uri;
        }

        public static string AbsolutePath()
        {
            return AspNetCoreHttpContext.Current.Request.Path.Value;
        }

        public static string UrlReferrer()
        {
            return AspNetCoreHttpContext.Current.Request.Headers["Referer"].ToString();
        }

        public static string RouteData(string name)
        {
            return AspNetCoreHttpContext.Current
                 .GetRouteData()?
                 .Values[name]?
                 .ToString()
                 ?? string.Empty;
        }

        public static string Encode(string url)
        {
            return HttpUtility.UrlEncode(url);
        }
    }
}