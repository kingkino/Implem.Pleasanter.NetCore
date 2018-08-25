using AspNetCoreCurrentRequestContext;
using Implem.Pleasanter.Libraries.Server;
using Implem.Pleasanter.Libraries.Settings;
using System.Web;
namespace Implem.Pleasanter.Libraries.Requests
{
    public static class Views
    {
        public static View GetBySession(SiteSettings ss)
        {
            var key = "View" + (ss.SiteId == 0
                ? Pages.Key()
                : ss.SiteId.ToString());
            if (Forms.ControlId() == "ViewSelector")
            {
                var view = ss.Views?.Get(Forms.Int("ViewSelector")) ?? new View(ss);
                Sessions.Set(key, view);
                return view;
            }
            else if (Sessions.Get<View>(key) != null)
            {
                var view = Sessions.Get<View>(key);
                view.SetByForm(ss);
                return view;
            }
            else
            {
                var view = ss.Views?.Get(ss.GridView) ?? new View(ss);
                Sessions.Set(key, view);
                return view;
            }
        }
    }
}