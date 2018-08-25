using AspNetCoreCurrentRequestContext;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.Server;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Implem.Pleasanter.Libraries.Requests
{
    public static class Forms
    {
        public static IFormCollection Form()
        {
            var request = AspNetCoreHttpContext.Current.Request;
            var contentType = request.ContentType;
            if (string.IsNullOrEmpty(contentType)) request.ContentType = "application/x-www-form-urlencoded";
            return request.Form;

        }


        public static string String()
        {
            return HttpUtility.UrlDecode(Form().ToString(), System.Text.Encoding.UTF8);
        }

        public static Dictionary<string, string> All()
        {
            var hash = new Dictionary<string, string>();
            Form().Keys.ToList().ForEach(key =>
               hash.Add(key, Form()[key]));
            return hash;
        }

        public static string ControlId()
        {
            return Data("ControlId");
        }

        public static bool Bool(string key)
        {
            return Data(key).ToBool();
        }

        public static int Int(string key)
        {
            return Data(key).ToInt();
        }

        public static long Long(string key)
        {
            return Data(key).ToLong();
        }

        public static decimal Decimal(string key)
        {
            return Data(key).ToDecimal(Sessions.CultureInfo());
        }

        public static DateTime DateTime(string key)
        {
            return Data(key).ToDateTime();
        }

        public static List<int> IntList(string name)
        {
            return Data(name)
                .Deserialize<List<int>>()?
                .ToList() ?? new List<int>();
        }

        public static List<long> LongList(string name)
        {
            return Data(name)
                .Deserialize<List<long>>()?
                .ToList() ?? new List<long>();
        }

        public static List<string> List(string name)
        {
            return Data(name)
                .Deserialize<List<string>>()?
                .Where(o => o != string.Empty)
                .ToList() ?? new List<string>();
        }

        public static IEnumerable<string> Keys()
        {
            foreach(var key in Form().Keys)
                yield return key;
        }

        public static IEnumerable<string> FileKeys()
        {
            foreach (var file in Form().Files)
                yield return file.Name;
        }

        public static bool Exists(string key)
        {
            return HasData(key);
        }

        public static string Data(string key)
        {
            if (!Form().TryGetValue(key, out var value)) return string.Empty;
            var data = value.FirstOrDefault() ?? string.Empty;
            return data;
        }

        public static byte[] File(string key)
        {
            var file = Form().Files[key];
            if (file != null)
            {
                var bin = new byte[file.Length];
                using (var strem = file.OpenReadStream())
                    strem.Read(bin, 0, (int)file.Length);
                return bin;
            }
            else
            {
                return null;
            }
        }

        public static bool HasData(string key)
        {
            return Form().TryGetValue(key, out var value) ? value.Count != 0 : false;
        }
    }
}