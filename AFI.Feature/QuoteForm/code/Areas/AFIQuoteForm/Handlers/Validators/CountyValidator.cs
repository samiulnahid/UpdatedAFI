using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers.Validators
{
	public static class HomeownersCountyValidator
	{
		public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
		{
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.HomeownersQuote);
		}
	}

	public static class MobilehomesCountyValidator
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.MobilehomeQuote);
        }
    }

    public static class AutoCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.AutoQuote);
		}
    }

    public static class UmbrellaCountyValidator 
    {
		public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.UmbrellaQuote);
		}
    }

    public static class FloodCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.FloodQuote);
		}
    }

    public static class WatercraftCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.WatercraftQuote);
		}
    }

    public static class MotorcycleCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.MotorcycleQuote);
		}
    }

    public static class MotorhomeCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.MotorhomeQuote);
		}
    }

    public static class CollectorVehicleCountyValidator 
    {
        public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
        {
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.CollectorVehicle);

		}
    }

	public static class BusinessCountyValidator
	{
		public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
		{
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.Commercial);

		}
	}

	public static class RentersCountyValidator
	{
		public static bool Validate(IEnumerable<QuoteCountyFilter> counties, string countyName, string stateAbbreviation)
		{
			return counties.All(c => !string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) &&
									 !string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase))
									 || counties.Any(c => string.Equals(c.CountyName, countyName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(c.StateAbrev, stateAbbreviation, StringComparison.InvariantCultureIgnoreCase) && c.RentersQuote);

		}
	}
}