using System.Web.Mvc;

namespace AFI.Feature.QuoteForm.Areas.ShortForms
{
    public class ShortFormsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShortForms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ShortForms_default",
                "ShortForms/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}