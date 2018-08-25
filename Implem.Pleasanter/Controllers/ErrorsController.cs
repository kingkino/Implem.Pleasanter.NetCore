using Implem.DefinitionAccessor;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Filters;
using Implem.Pleasanter.Libraries.General;
using Implem.Pleasanter.Libraries.HtmlParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Implem.Pleasanter.Controllers
{
    [Authorize]
    [CheckContract]
    [RefleshSiteInfo]
    public class ErrorsController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!Libraries.Requests.Request.IsAjaxRequest(Request))
            {
                ViewBag.HtmlBody = HtmlTemplates.Error(Error.Types.ApplicationError);
                return View();
            }
            else
            {
                return Content(Error.Types.ApplicationError.MessageJson());
            }
        }

        [AllowAnonymous]
        public ActionResult BadRequest()
        {
            // Response.StatusCode = (int)HttpStatusCode.BadRequest;
            if (!Libraries.Requests.Request.IsAjaxRequest(Request))
            {
                ViewBag.HtmlBody = HtmlTemplates.Error(Error.Types.BadRequest);
                return View();
            }
            else
            {
                return Content(Error.Types.NotFound.MessageJson());
            }
        }

        [AllowAnonymous]
        public ActionResult NotFound()
        {
            // Response.StatusCode = (int)HttpStatusCode.NotFound;
            if (!Libraries.Requests.Request.IsAjaxRequest(Request))
            {
                ViewBag.HtmlBody = HtmlTemplates.Error(Error.Types.NotFound);
                return View();
            }
            else
            {
                return Content(Error.Types.NotFound.MessageJson());
            }
        }

        [AllowAnonymous]
        public ActionResult ParameterSyntaxError()
        {
            var messageData = new string[]
            {
                Parameters.SyntaxErrors?.Join(",")
            };
            if (!Libraries.Requests.Request.IsAjaxRequest(Request))
            {
                ViewBag.HtmlBody = HtmlTemplates.Error(
                    Error.Types.ParameterSyntaxError,
                    messageData);
                return View();
            }
            else
            {
                return Content(Error.Types.ParameterSyntaxError
                    .MessageJson(messageData));
            }
        }

        [AllowAnonymous]
        public ActionResult InternalServerError()
        {
            // Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (!Libraries.Requests.Request.IsAjaxRequest(Request))
            {
                ViewBag.HtmlBody = HtmlTemplates.Error(Error.Types.InternalServerError);
                return View();
            }
            else
            {
                return Content(Error.Types.InternalServerError.MessageJson());
            }
        }
    }
}
