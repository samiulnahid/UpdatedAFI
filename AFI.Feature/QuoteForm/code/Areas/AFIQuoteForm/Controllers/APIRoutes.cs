using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Controllers
{
    public static class APIRoutes
    {
        public const string API_ROUTE_PREFIX = "sc-api/forms";
        public const string GET_AUTO_FORM = "GetAutoForm";
        public const string RETURN_AUTO_FORM = "GetAutoForm/{key}";
        public const string GET_WATERCRAFT_FORM = "GetWatercraftForm";
        public const string RETURN_WATERCRAFT_FORM = "GetWatercraftForm/{key}";
        public const string GET_MOTORCYCLE_FORM = "GetMotorcycleForm";
        public const string RETURN_MOTORCYCLE_FORM = "GetMotorcycleForm/{key}";
        public const string RETURN_BUSINESS_FORM = "GetBusinessForm/{key}";
        public const string GET_BUSINESS_FORM = "GetBusinessForm";
        public const string GET_HILLAFB_FORM = "GetHillAFBForm";
        public const string RETURN_HillAFB_FORM = "GetHillAFBForm/{key}";
        public const string GET_UMBRELLA_FORM = "GetUmbrellaForm";
        public const string RETURN_UMBRELLA_FORM = "GetUmbrellaForm/{key}";
        public const string GET_FLOOD_FORM = "GetFloodForm";
        public const string RETURN_FLOOD_FORM = "GetFloodForm/{key}";
        public const string GET_RENTER_FORM = "GetRenterForm";
        public const string RETURN_RENTER_FORM = "GetRenterForm/{key}";
        public const string GET_COLLECTOR_FORM = "GetCollectorVehicleForm";
        public const string RETURN_COLLECTOR_FORM = "GetCollectorVehicleForm/{key}";
        public const string GET_MOTORHOME_FORM = "GetMotorhomeForm";
        public const string RETURN_MOTORHOME_FORM = "GetMotorhomeForm/{key}";
        public const string GET_HOMEOWNER_FORM = "GetHomeownerForm";
        public const string GET_HOMENONOWNER_FORM = "GetHomenonownerForm";
        public const string GET_MOBILEHOME_FORM = "GetMobilehomeForm";
        public const string SAVE_AUTO_FORM = "save-auto";
        public const string SAVE_COLLECTOR_FORM = "save-collectorvehicle";
        public const string SAVE_WATERCRAFT_FORM = "save-watercraft";
        public const string SAVE_MOTORCYCLE_FORM = "save-motorcycle";
        public const string SAVE_MOTORHOME_FORM = "save-motorhome";
		public const string SAVE_FLOOD_FORM = "save-flood";
		public const string SAVE_HOMEOWNER_FORM = "save-homeowner";
		public const string SAVE_HOMENONOWNER_FORM = "save-homenonowner";
		public const string SAVE_MOBILEHOME_FORM = "save-mobilehome";
		public const string SAVE_RENTER_FORM = "save-renter";
        public const string SAVE_BUSINESS_FORM = "save-business";
        public const string SAVE_HillAFB_FORM = "save-hillafb";
        public const string SAVE_UMBRELLA_FORM = "save-umbrella";
		public const string GET_PRODUCT_AVAILABILITY = "GetProductAvailability";
        public const string GET_AUTO_BY_VIN = "getvin";
        public const string GET_AUTO_MAKE_BY_YEAR = "getmake";
        public const string GET_AUTO_MODEL_BY_MAKE = "getmodel";
        public const string GET_AUTO_BODY_STYLES_BY_MODEL = "getbodytype";
        public const string GET_LEADGENERATION_FORM = "GetLeadGenerationForm";
        public const string RETURN_LEADGENERATION_FORM = "GetLeadGenerationForm/{key}";
        public const string SAVE_LEADGENERATION_FORM = "save-leadgeneration";
        public const string GET_CONDO_FORM = "GetCondoForm";
        public const string RETURN_CONDO_FORM = "GetCondoForm/{key}";
        public const string GET_CONDO_FORM_N = "GetCondoNFormData";
        public const string SAVE_CONDO_FORM = "save-condo";
        public const string SAVE_QuoteLead_FORM = "save-quotelead";
        public const string GET_QuoteLead_FORM = "GetQuoteLeadForm";
        public const string RETURN_QuoteLead_FORM = "GetQuoteLeadForm/{key}";
    }
}