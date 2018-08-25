using AspNetCoreCurrentRequestContext;
using Implem.Libraries.Utilities;
using System;
using System.Web;
namespace Implem.Pleasanter.Libraries.Server
{
    public static class Applications
    {
        public static DateTime SysLogsMaintenanceDate = DateTime.Now;
        public static DateTime SearchIndexesMaintenanceDate = DateTime.Now;

        public static DateTime StartTime { get; private set; }
        public static DateTime LastAccessTime { get; set; }

        public static void SetStartTime(DateTime dateTime) => StartTime = dateTime;

        public static double ApplicationAge()
        {
            return (DateTime.Now - StartTime)
                .TotalMilliseconds;
        }

        public static double ApplicationRequestInterval()
        {
            var ret = (DateTime.Now - LastAccessTime)
                .TotalMilliseconds;
            LastAccessTime = DateTime.Now;
            return ret;
        }
    }
}