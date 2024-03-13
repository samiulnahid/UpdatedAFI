using System.Web.Mvc;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB
{
    public class AFIWEBAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AFIWEB";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AFIWEB_default",
                "AFIWEB/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}