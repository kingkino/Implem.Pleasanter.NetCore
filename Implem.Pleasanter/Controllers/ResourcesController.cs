using Implem.Pleasanter.Filters;
using Implem.Pleasanter.Libraries.Resources;
using Implem.Pleasanter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Implem.Pleasanter.Controllers
{
    [Authorize]
    [CheckContract]
    [RefleshSiteInfo]
    public class ResourcesController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ContentResult Scripts()
        {
            var log = new SysLogModel();
            var result = JavaScripts.Get();
            log.Finish(result.Content.Length);
            return result;
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ContentResult Styles()
        {
            var log = new SysLogModel();
            var result = Css.Get();
            log.Finish(result.Content.Length);
            return result;
        }
    }
}