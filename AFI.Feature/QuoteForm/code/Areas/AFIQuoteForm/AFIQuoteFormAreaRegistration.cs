using System.Web.Mvc;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm
{
    public class AFIQuoteFormAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AFIQuoteForm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AFIQuoteForm_default",
                "AFIQuoteForm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}