using Implem.Libraries.Utilities;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreCurrentRequestContext;
using System;

namespace Implem.Pleasanter.Libraries.Requests
{
    public class Request
    {
        public HttpRequest HttpRequest;

        public Request()
        {
            HttpRequest = AspNetCoreHttpContext.Current != null && AspNetCoreHttpContext.Current.User != null
                ? AspNetCoreHttpContext.Current.Request
                : null;
        }

        public static bool IsAjax()
        {
            return IsAjaxRequest(AspNetCoreHttpContext.Current.Request);
        }

        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }


        public bool IsMobile()
        {
            return HttpRequest?.Headers.TryGetValue("User-Agent", out var value) == true ? value.Any(v => v.Contains("Mobile")) : false;
        }

        public string ProcessedRequestData()
        {
            return Forms.Form()
                .Where(f => f.Value.Any(v => !string.IsNullOrEmpty(v)))
                .Select(f => $"{f.Key}={f.Value.Join(",")}")
                .Select(o => ProcessedRequestData(o))
                .Join("&");
        }

        private string ProcessedRequestData(string requestData)
        {
            switch (requestData.Substring(0, requestData.IndexOf("=")).ToLower())
            {
                case "users_password": return "Users_Password=*";
                case "users_changedpassword": return "Users_ChangedPassword=*";
                case "users_afterresetpassword": return "Users_AfterResetPassword=*";
                default: return requestData;
            }
        }

        public string HttpMethod()
        {
            return HttpRequest != null
               ? HttpRequest.Method
               : null;
        }

        public string Url()
        {
            return HttpRequest != null
               ? $"{HttpRequest.Scheme}://{HttpRequest.Host.Value}{HttpRequest.Path.Value}{HttpRequest.QueryString.Value}"
               : null;
        }

        public string UrlReferrer()
        {
            return HttpRequest.Headers.TryGetValue("Referer", out var value)
                ? value.FirstOrDefault().ToString()
                : null;
        }

        public string UserHostName()
        {
            return HttpRequest.HttpContext.Connection.RemoteIpAddress != null
               ? HttpRequest.HttpContext.Connection.RemoteIpAddress.ToString()
               : null;
        }

        public string UserHostAddress()
        {
            return HttpRequest.HttpContext.Connection.RemoteIpAddress != null
               ? HttpRequest.HttpContext.Connection.RemoteIpAddress.ToString()
               : null;
        }

        public string UserLanguage()
        {
            return HttpRequest.Headers.TryGetValue("Accept-Language", out var value)
                ? value.FirstOrDefault()
                : null;
        }

        public string UserAgent()
        {
            return HttpRequest.Headers.TryGetValue("User-Agent", out var value)
                ? value.FirstOrDefault()
                : null;
        }
    }
}