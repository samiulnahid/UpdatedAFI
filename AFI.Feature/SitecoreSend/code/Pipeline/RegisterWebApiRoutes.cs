using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace AFI.Feature.SitecoreSend.Pipeline
{
    public class RegisterWebApiRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("AFI.Feature.SitecoreSend",
                                       "api/sitecoresend/{action}",
                                       new { controller = "Audience" });
            
        }
    }
}