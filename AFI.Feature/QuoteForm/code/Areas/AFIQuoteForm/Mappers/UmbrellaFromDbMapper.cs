using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class UmbrellaFromDbMapper
    {

        public static void Map(this QuoteUmbrella quote, UmbrellaFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteUmbrella();
            }

            quote.Key = quoteKey;
            quote.CoverageAmount = model.QuoteAmount;
            quote.UnderlyingInsurance = model.UnderlyingPersonalLiabilityLimits;
            quote.NumberOfDrivers = model.NumberOfDrivers;
            quote.DriversUnder22 = model.NumberOfDriversUnderAge22;
            quote.DriversUnder25 = model.NumberOfDriversUnderAge25;
            quote.DriversOver70 = model.NumberOfDriversOverAge70;
            quote.NumberOfVehicles = model.NumberOfVehicles;
            quote.VehicleUnderlyingInsurance = model.AutoLiabilityCoverageLimits;
            quote.OwnRentalProperty = model.DoYouOwnAnyRentalProperty;
            quote.NumberOfRentalProperties = model.NumberOfRentalProperties;
            quote.RentalUnderlyingInsurance = model.RentalUnderlyingInsurance;

        }

        public static void Map(this QuoteUmbrellaVehicle quote, UmbrellaFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteUmbrellaVehicle();
            }

            quote.QuoteKey = quoteKey;
            quote.VehicleType = model.Type0OfRecreationalVehicle;
            quote.VehicleUnderlyingInsurance = model.Recreational0VehicleLiability;
        }

        public static void Map(this QuoteUmbrellaWatercraft quote, UmbrellaFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteUmbrellaWatercraft();
            }

            quote.QuoteKey = quoteKey;
            quote.WatercraftType = model.Type0OfWatercraft;
            quote.WatercraftLength = model.Length0OfWatercraft;
            quote.WatercraftHorsepower = model.Horsepower0OfWatercraft;
            quote.WatercraftUnderlyingInsurance = model.Watercraft0Liability;
        }

        public static void ReverseMap(this UmbrellaFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteUmbrella umbrella, List<QuoteUmbrellaVehicle> umbrellaVehicles, List<QuoteUmbrellaWatercraft> umbrellaWatercrafts, List<QuoteAutoIncident> autoIncidents)
        {
            if (model == null)
            {
                model = new UmbrellaFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, autoIncidents, null);

            if (umbrella != null)
            {
                model.QuoteAmount = umbrella.CoverageAmount;
                model.UnderlyingPersonalLiabilityLimits = umbrella.UnderlyingInsurance;
                model.NumberOfDrivers = umbrella.NumberOfDrivers;
                model.NumberOfDriversUnderAge22 = umbrella.DriversUnder22;
                model.NumberOfDriversUnderAge25 = umbrella.DriversUnder25;
                model.NumberOfDriversOverAge70 = umbrella.DriversOver70;
                model.NumberOfVehicles = umbrella.NumberOfVehicles;
                model.AutoLiabilityCoverageLimits = umbrella.VehicleUnderlyingInsurance;
                model.DoYouOwnAnyRentalProperty = umbrella.OwnRentalProperty;
                model.NumberOfRentalProperties = umbrella.NumberOfRentalProperties;
                model.RentalUnderlyingInsurance = umbrella.RentalUnderlyingInsurance;
            }

            if (umbrellaVehicles != null && umbrellaVehicles.Any())
            {
                for (int i = 0; i < umbrellaVehicles.Count; i++)
                {
                    model.SetPropertyValueByIndex("Type", "OfRecreationalVehicle", i, umbrellaVehicles[i].VehicleType);
                    model.SetPropertyValueByIndex("Recreational", "VehicleLiability", i, umbrellaVehicles[i].VehicleType);
                }
            }

            if (umbrellaWatercrafts != null && umbrellaWatercrafts.Any())
            {
                for (int i = 0; i < umbrellaWatercrafts.Count; i++)
                {
                    model.SetPropertyValueByIndex("Type", "OfWatercraft", i, umbrellaWatercrafts[i].WatercraftType);
                    model.SetPropertyValueByIndex("Length", "OfWatercraft", i, umbrellaWatercrafts[i].WatercraftLength);
                    model.SetPropertyValueByIndex("Horsepower", "OfWatercraft", i, umbrellaWatercrafts[i].WatercraftHorsepower);
                    model.SetPropertyValueByIndex("Watercraft", "Liability", i, umbrellaWatercrafts[i].WatercraftUnderlyingInsurance);
                }
            }
        }
    }
}