using AspNetCoreCurrentRequestContext;
using Implem.Libraries.Classes;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.DataSources;
using Implem.Pleasanter.Libraries.DataTypes;
using Implem.Pleasanter.Libraries.HtmlParts;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Libraries.Security;
using Implem.Pleasanter.Libraries.Settings;
using Implem.Pleasanter.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
namespace Implem.Pleasanter.Libraries.Server
{
    public static class Sessions
    {
        public static string Data(string name)
        {
            return Get<string>(name) ?? string.Empty;
        }

        public static void Set(string name, object data)
        {
            AspNetCoreHttpContext.Current.Session.Set(name, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            //TODO SessionWrapper.Set(name, data);
        }

        public static T Get<T>(string name)
        {
            if (!AspNetCoreHttpContext.Current.Session.TryGetValue(name, out var bytes)) return default(T);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }

        public static void Abandon()
        {
            AspNetCoreHttpContext.Current.Session.Clear();
        }

        public static void Clear(string name)
        {
            Set(name, null);
        }

        public static bool Created()
        {
            return AspNetCoreHttpContext.Current?.Session != null;
        }

        public static void SetTenantId(int tenantId)
        {
            Set("TenantId", tenantId);
            SiteInfo.Reflesh();
        }

        public static void Set(int tenantId, int userId)
        {
            Set("TenantId", tenantId);
            Set("RdsUser",
                Rds.ExecuteTable(statements: Rds.SelectUsers(
                    column: Rds.UsersColumn().UserId().DeptId(),
                    where: Rds.UsersWhere().UserId(userId)))
                        .AsEnumerable()
                        .Select(dataRow => new RdsUser()
                        {
                            DeptId = dataRow.Int("DeptId"),
                            UserId = dataRow.Int("UserId")
                        })
                        .FirstOrDefault());
            if (!SiteInfo.TenantCaches.ContainsKey(TenantId()))
            {
                SiteInfo.Reflesh();
            }
        }

        public static int TenantId()
        {
            return Get<int>("TenantId");
        }

        public static bool LoggedIn()
        {
            return
                AspNetCoreHttpContext.Current?.User?.Identity.Name.IsNullOrEmpty() == false &&
                AspNetCoreHttpContext.Current?.User.Identity.Name !=
                    Implem.Libraries.Classes.RdsUser.UserTypes.Anonymous.ToInt().ToString();
        }

        private static int UserIdentity()
        {
            var id = Get<int>("UserId");
            if (id != 0)
            {
                return id.ToInt();
            }
            else
            {
                var name = AspNetCoreHttpContext.Current?.User.Identity.Name;
                var userId = Authentications.Windows() && name != null
                    ? Rds.ExecuteScalar_int(statements:
                        Rds.SelectUsers(
                            column: Rds.UsersColumn().UserId(),
                            where: Rds.UsersWhere().LoginId(name)))
                    : name.ToInt();
                Set("UserId", userId);
                return userId;
            }
        }

        public static int UserId()
        {
            return LoggedIn()
                ? UserIdentity()
                : Implem.Libraries.Classes.RdsUser.UserTypes.Anonymous.ToInt();
        }

        public static int DeptId()
        {
            return LoggedIn() ? SiteInfo.User(UserIdentity()).DeptId : 0;
        }

        public static User User()
        {
            return SiteInfo.User(UserId());
        }

        public static RdsUser RdsUser()
        {
            return Get<RdsUser>("RdsUser");
        }

        public static string Language()
        {
            return (Created() ? Get<string>("Language") : default(string)) ?? string.Empty;
        }

        public static CultureInfo CultureInfo()
        {
            return new CultureInfo(Language());
        }

        public static bool Developer()
        {
            return Get<bool>("Developer");
        }

        public static TimeZoneInfo TimeZoneInfo()
        {
            return Get<TimeZoneInfo>("TimeZoneInfo") ?? Environments.TimeZoneInfoDefault;
        }

        public static UserSettings UserSettings()
        {
            return Get<string>("UserSettings")?.Deserialize<UserSettings>() ?? new UserSettings();
        }

        public static double SessionAge()
        {
            return Created() ? (DateTime.Now - Get<DateTime>("StartTime")).TotalMilliseconds : 0;
        }

        public static double SessionRequestInterval()
        {
            if (Created())
            {
                var ret = (DateTime.Now - Get<DateTime>("LastAccessTime")).TotalMilliseconds;
                Set("LastAccessTime", DateTime.Now);
                return ret;
            }
            else
            {
                return 0;
            }
        }

        public static string SessionGuid()
        {
            return Get<string>("SessionGuid");
        }

        public static Message Message()
        {
            var message = Get<Message>("Message");
            if (message != null) Clear("Message");
            return message;
        }

        public static object PageSession(this BaseModel baseModel, string name)
        {
            return Get<object>(Pages.Key(baseModel, name));
        }

        public static void PageSession(this BaseModel baseModel, string name, object value)
        {
            Set(Pages.Key(baseModel, name), value);
        }
    }
}