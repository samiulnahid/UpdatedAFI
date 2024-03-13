using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorVehicleForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class CollectorVehicleFormDbMapper
    {
        public static void Map(this QuoteAuto quote, CollectorVehicleFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAuto();
            }

            quote.Key = quoteKey;
            quote.BodilyInjury = model.BodilyInjuryLiability;
            quote.PropertyDamage = model.PropertyDamageLiability;
            quote.MedicalCoverage = model.MedicalPayment;
            quote.UninsuredBodilyInjury = model.UninsuredMotoristBodilyInjury;
            quote.PersonalInjury = model.PersonalInjuryProtection;
            quote.CurrentInsurance = string.Equals(model.CurrentInsuranceCompany, "Other", StringComparison.InvariantCultureIgnoreCase) ? model.OtherInsuranceCompany : model.CurrentInsuranceCompany;
            quote.CurrentPolicyDate = model.PolicyRenewalDate;
            quote.CurrentPolicyAction = model.PolicyRenewalType;
            //INFO: Per comment here https://degdigital.atlassian.net/browse/AFI-641?focusedCommentId=180044 Vehicle index 0 is being used
            quote.NumberOfLicensedDrivers = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, int?>("Vehicle", "LicensedDriverCount", 0);
            quote.NumberOfDailyUseVehiclesInHousehold = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, int?>("Vehicle", "NumberOfDailyUseVehiclesInHousehold", 0);
            quote.ComprehensiveDeductible  = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>("Vehicle", "ComprehensiveDeductible", 0);
            quote.CollisionDeductible  = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>("Vehicle", "CollisionDeductible", 0);
        }

        public static void Map(this QuoteCollectorVehicle quote, CollectorVehicleFormSaveModel model, int quoteKey, int vehicleIndex)
        {
            if (quote == null)
            {
                quote = new QuoteCollectorVehicle();
            }

            quote.QuoteKey = quoteKey;
            quote.Key = vehicleIndex;
            string vehicle = "Vehicle";
            quote.Year = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, int?>(vehicle, "Year", vehicleIndex);
            quote.Make = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "Make", vehicleIndex);
            quote.Model = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "Model", vehicleIndex);
            quote.Type = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "Type", vehicleIndex);
            quote.EstimatedValue = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, int?>(vehicle, "EstimatedValue", vehicleIndex);
            quote.VehicleStorage = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "Storage", vehicleIndex);
            quote.DescribeHowVehicleIsDriven = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "DriveDescription", vehicleIndex);
            quote.ComprehensiveDeductible = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "ComprehensiveDeductible", vehicleIndex);
            quote.CollisionDeductible = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, string>(vehicle, "CollisionDeductible", vehicleIndex);
            quote.LiabilityOnly = model.GetPropertyValueByIndex<CollectorVehicleFormSaveModel, bool>(vehicle, "LiabilityOnlyCoverage", vehicleIndex);
        }

        public static void ReverseMap(this CollectorVehicleFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteAuto auto, List<QuoteCollectorVehicle> vehicles, List<QuoteAutoIncident> incidents)
        {
            if (model == null)
            {
                model = new CollectorVehicleFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, incidents, auto);

            if (vehicles != null && vehicles.Any())
            {
                string prefix = "Vehicle";
                foreach (var vehicle in vehicles)
                {
                    int index = vehicles.IndexOf(vehicle);
                    model.SetPropertyValueByIndex(prefix, "Year", index, vehicle.Year);
                    model.SetPropertyValueByIndex(prefix, "Make", index, vehicle.Make);
                    model.SetPropertyValueByIndex(prefix, "Model", index, vehicle.Model);
                    model.SetPropertyValueByIndex(prefix, "Type", index, vehicle.Type);
                    model.SetPropertyValueByIndex(prefix, "EstimatedValue", index, vehicle.EstimatedValue);
                    model.SetPropertyValueByIndex(prefix, "VehicleStorage", index, vehicle.VehicleStorage);
                    model.SetPropertyValueByIndex(prefix, "DriverDescription", index, vehicle.DescribeHowVehicleIsDriven);
                    model.SetPropertyValueByIndex(prefix, "ComprehensiveDeductible", index, vehicle.ComprehensiveDeductible);
                    model.SetPropertyValueByIndex(prefix, "CollisionDeductible", index, vehicle.CollisionDeductible);
                    model.SetPropertyValueByIndex(prefix, "LiabilityOnlyCoverage", index, vehicle.LiabilityOnly);
                }
            }
        }
    }
}