using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers
{
    public class EnableCorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "*");

            base.OnActionExecuting(filterContext);
        }
    }
}