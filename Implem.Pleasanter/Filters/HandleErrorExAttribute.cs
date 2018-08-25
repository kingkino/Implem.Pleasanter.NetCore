using Implem.Pleasanter.Libraries.Responses;
using Implem.Pleasanter.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Implem.Pleasanter.Filters
{
    public class HandleErrorExAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("No ExceptionContext");
            }
            try
            {
                new SysLogModel(filterContext);
            }
            catch
            {
                throw;
            }
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult(Locations.ApplicationError());
        }
    }
}
