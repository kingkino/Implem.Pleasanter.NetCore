using Implem.Libraries.Utilities;
using Implem.Pleasanter.Filters;
using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Implem.Pleasanter.Controllers
{
    [Authorize]
    [CheckContract]
    [RefleshSiteInfo]
    public class BinariesController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ActionResult SiteImageThumbnail(string reference, long id)
        {
            if (reference.ToLower() == "items")
            {
                var bytes = BinaryUtilities.SiteImageThumbnail(new SiteModel(id));
                return new FileContentResult(bytes, "image/png");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ActionResult SiteImageIcon(string reference, long id)
        {
            if (reference.ToLower() == "items")
            {
                var bytes = BinaryUtilities.SiteImageIcon(new SiteModel(id));
                return new FileContentResult(bytes, "image/png");
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public string UpdateSiteImage(string reference, long id)
        {
            var log = new SysLogModel();
            var json = reference.ToLower() == "items"
                ? BinaryUtilities.UpdateSiteImage(new SiteModel(id))
                : new ResponseCollection().ToJson();
            log.Finish(0);
            return json;
        }

        [HttpDelete]
        public string DeleteSiteImage(string reference, long id)
        {
            var log = new SysLogModel();
            var json = reference.ToLower() == "items"
                ? BinaryUtilities.DeleteSiteImage(new SiteModel(id))
                : new ResponseCollection().ToJson();
            log.Finish(0);
            return json;
        }

        [HttpPost]
        public string UploadImage(string reference, long id, ICollection<IFormFile> file)
        {
            var log = new SysLogModel();
            var json = BinaryUtilities.UploadImage(file, id);
            log.Finish(json.Length);
            return json;
        }

        [HttpDelete]
        public string DeleteImage(string reference, string guid)
        {
            var log = new SysLogModel();
            var json = BinaryUtilities.DeleteImage(guid);
            log.Finish(json.Length);
            return json;
        }

        [HttpPost]
        public string MultiUpload(string reference, long id, ICollection<IFormFile> file)
        {
            var log = new SysLogModel();
            var json = BinaryUtilities.MultiUpload(file, id);
            log.Finish(json.Length);
            return json;
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public FileContentResult Download(string reference, string guid)
        {
            var log = new SysLogModel();
            var file = BinaryUtilities.Donwload(guid);
            log.Finish(file?.FileContents.Length ?? 0);
            return file;
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public FileContentResult DownloadTemp(string reference, string guid)
        {
            var log = new SysLogModel();
            var file = BinaryUtilities.DownloadTemp(guid);
            log.Finish(file?.FileContents.Length ?? 0);
            return file;
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ActionResult Show(string reference, string guid)
        {
            var log = new SysLogModel();
            var file = BinaryUtilities.Donwload(guid);
            log.Finish(file?.FileContents.Length ?? 0);
            return file != null
                ? File(file.FileContents, file.ContentType)
                : null;
        }

        [HttpGet]
        [ResponseCache(Duration = int.MaxValue)]
        public ActionResult ShowTemp(string reference, string guid)
        {
            var log = new SysLogModel();
            var file = BinaryUtilities.DownloadTemp(guid);
            log.Finish(file?.FileContents.Length ?? 0);
            return file != null
                ? File(file.FileContents, file.ContentType)
                : null;
        }

        [HttpPost]
        public string DeleteTemp(string reference, long id)
        {
            var log = new SysLogModel();
            var json = BinaryUtilities.DeleteTemp();
            log.Finish(json.Length);
            return json.ToString();
        }
    }
}