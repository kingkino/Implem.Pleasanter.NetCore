using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Implem.Pleasanter.Libraries.Responses
{
    public static class VirtualPathUtility
    {
        public static string ToAbsolute(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            if (path.IndexOf('~') < 0) return path; ;
            return path.Replace("~", AspNetCoreCurrentRequestContext.AspNetCoreHttpContext.Current.Request.PathBase);
        }
    }
}
