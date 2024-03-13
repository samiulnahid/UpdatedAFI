using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers.Validators
{

	public static class AutoStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.AutoQuote);
        }
    }

    public static class MotorcycleStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.MotorcycleQuote);
        }
    }
	public static class HomeownersStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
			return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.HomeownersQuote);
		}
	}

	public static class MobilehomesStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.MobilehomesQuote);
        }
    }

    public static class UmbrellaStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.UmbrellaQuote);
        }
    }

    public static class FloodStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.FloodQuote);
        }
    }

    public static class WatercraftStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.WatercraftQuote);
        }
    }

    public static class MotorhomeStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.MotorhomeQuote);
        }
    }

    public static class CollectorVehicleStateValidator
	{

        public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
        {
            return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.CollectorVehicle);
        }
    }
	public static class HomenonownersStateValidator
	{

		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
			return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.CollectorVehicle);
		}
	}
	public static class CondoStateValidator
	{

		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
			return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.CollectorVehicle);
		}
	}
	public static class BusinessStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
			return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.Commercial);
		}
	}

	public static class RentersStateValidator
	{
		public static bool Validate(IEnumerable<QuoteState> states, string stateAbbreviation)
		{
			return states.Any(s => s.QuoteStateAbbreviation == stateAbbreviation && s.RentersQuote);
		}
	}
}
