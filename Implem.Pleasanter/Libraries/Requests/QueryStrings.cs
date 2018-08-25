using AspNetCoreCurrentRequestContext;
using Implem.Libraries.Utilities;
using System.Linq;
using System.Web;
namespace Implem.Pleasanter.Libraries.Requests
{
    public class QueryStrings
    {
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

        public static string Data(string key)
        {
            return AspNetCoreHttpContext.Current.Request.Query.TryGetValue(key, out var value)
                ? value.FirstOrDefault()
                : string.Empty;
        }
    }
}