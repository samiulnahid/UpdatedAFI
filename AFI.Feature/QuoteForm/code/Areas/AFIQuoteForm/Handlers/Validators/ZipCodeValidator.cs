using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers.Validators
{
    public static class AutoZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.AutoQuote);
        }
    }

    public static class MobilehomeZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.MobilehomesQuote);
        }
    }
    public static class HomeownersZipCodeValidator
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.HomeownersQuote);
        }
    }

    public static class HomenonownersZipCodeValidator
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.HomeownersQuote);
        }
    }
    public static class CondoZipCodeValidator
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.CondoQuote);
        }
    }

    public static class UmbrellaZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.UmbrellaQuote);
        }
    }

    public static class FloodZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.FloodQuote);
        }
    }

    public static class WatercraftZipCodeValidator 
    {
       public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.WatercraftQuote);
        }
    }

    public static class MotorcycleZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.MotorcycleQuote);
        }
    }

    public static class MotorhomeZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.MotorhomeQuote);
        }
    }

    public static class CollectorVehicleZipCodeValidator 
    {
        public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
        {
            return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.CollectorVehicle);
        }
    }

	public static class BusinessZipCodeValidator
	{
		public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
		{
			return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.Commercial);
		}
	}

	public static class RentersZipCodeValidator
	{
		public static bool Validate(IEnumerable<QuoteZipCodeFilter> zipCodes, string zipCode)
		{
			return zipCodes.All(z => z.QuoteZipCode != zipCode) || zipCodes.Any(zc => zc.QuoteZipCode == zipCode && zc.RentersQuote);
		}
	}
}