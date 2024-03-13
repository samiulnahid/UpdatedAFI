using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class WatercraftFormDbMapper
    {
        private static readonly int DefaultSpouseCohabitantDriverKey = 5;
        public static void Map(this QuoteWatercraftVehicle quote, WatercraftFormSaveModel model, int quoteKey, int vehicleIndex)
        {
            if (quote == null)
            {
                quote = new QuoteWatercraftVehicle();
            }

            quote.QuoteKey = quoteKey;
            quote.Key = vehicleIndex;
            string watercraftVehiclePrefix = "Vehicle";
            quote.KeptAtResidence = model.GetPropertyValueByIndex<WatercraftFormSaveModel, bool>(watercraftVehiclePrefix, "AddressSameAsMailing", vehicleIndex);
            quote.GaragingZipCode = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "Zip", vehicleIndex);
            quote.PurchaseYear = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "YearPurchased", vehicleIndex);
            quote.HullMaterial = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "HullMaterials", vehicleIndex);
            quote.NumberOfMotors = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int?>(watercraftVehiclePrefix, "MotorCount", vehicleIndex) ?? 0;
            quote.TotalHorsepower = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int?>(watercraftVehiclePrefix, "Horsepower", vehicleIndex);
            quote.PropulsionType = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "PropulsionType", vehicleIndex);
            quote.MaxSpeed = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "MaxSpeed", vehicleIndex);
            quote.Value = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int>(watercraftVehiclePrefix, "Value", vehicleIndex);
            quote.Year = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int>(watercraftVehiclePrefix, "Year", vehicleIndex);
            quote.Make = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "Make", vehicleIndex);
            quote.Model = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "Model", vehicleIndex);
            quote.IncludesTrailer = model.GetPropertyValueByIndex<WatercraftFormSaveModel, bool>(watercraftVehiclePrefix, "TrailerIncluded", vehicleIndex);
            quote.TrailerValue = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int?>(watercraftVehiclePrefix, "TrailerValue", vehicleIndex) ?? 0;
            quote.BodyStyle = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "Style", vehicleIndex);
            quote.LengthInFeet = model.GetPropertyValueByIndex<WatercraftFormSaveModel, float?>(watercraftVehiclePrefix, "Length", vehicleIndex);
            quote.ComprehensiveDeductible = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "ComprehensiveDeductible", vehicleIndex);
            quote.CollisionDeductible = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "CollisionDeductible", vehicleIndex);
            quote.LiabilityOnly = model.GetPropertyValueByIndex<WatercraftFormSaveModel, bool?>(watercraftVehiclePrefix, "LiabilityOnlyCoverage", vehicleIndex);

            quote.GaragingStreet = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "Address", vehicleIndex);
            quote.GaragingCity = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "City", vehicleIndex);
            quote.GaragingState = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(watercraftVehiclePrefix, "State", vehicleIndex);
        }

        public static void Map(this QuoteAuto quote, WatercraftFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAuto();
            }

            quote.Key = quoteKey;
            quote.BodilyInjury = model.BodilyInjuryPropertyDamage;
            quote.MedicalCoverage = model.MedicalCoverage;
            quote.UninsuredBodilyInjury = model.UninsuredBoatersCoverage;
            quote.CurrentPolicyAction = model.PolicyRenewalType;
        }

        public static void MapCohabitant(this QuoteAutoDriver quote, WatercraftFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = 1;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.CohabitantFirstName;
            quote.LastName = model.CohabitantLastName;
            quote.Gender = model.CohabitantGender;
            quote.MaritalStatus = model.CohabitantMaritalStatus;
            quote.BirthDate = model.CohabitantDob;
            quote.AgeLicensed = model.CohabitantAgeWhenLicensed;
            quote.Occupation = model.CohabitantOccupation;
            quote.Education = model.CohabitantEducation;
            quote.ExperienceYears = model.CohabitantOperatingExperience;
        }

        public static void MapPolicyHolder(this QuoteAutoDriver quote, WatercraftFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = 0;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.PolicyHolderFirstName;
            quote.LastName = model.PolicyHolderLastName;
            quote.Gender = model.PolicyHolderGender;
            quote.MaritalStatus = model.PolicyHolderMaritalStatus;
            quote.BirthDate = model.PolicyHolderDob;
            quote.ExperienceYears = model.PolicyHolderOperatingExperience;
        }

        public static void MapDriver(this QuoteAutoDriver quote, WatercraftFormSaveModel model, int quoteKey, int driverIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = driverIndex + 2;
            quote.QuoteKey = quoteKey;
            string driverPrefix = "AdditionalDriver";
            quote.FirstName = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(driverPrefix, "FirstName", driverIndex);
            quote.LastName = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(driverPrefix, "LastName", driverIndex);
            quote.Gender = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(driverPrefix, "Gender", driverIndex);
            quote.MaritalStatus = model.GetPropertyValueByIndex<WatercraftFormSaveModel, string>(driverPrefix, "MaritalStatus", driverIndex);
            quote.BirthDate = model.GetPropertyValueByIndex<WatercraftFormSaveModel, DateTime?>(driverPrefix, "Dob", driverIndex);
            quote.ExperienceYears = model.GetPropertyValueByIndex<WatercraftFormSaveModel, int?>(driverPrefix, "OperatingExperience", driverIndex);
        }

        public static void ReverseMap(this WatercraftFormSaveModel model, FTData.Quote quote, QuoteContact contact, List<QuoteWatercraftVehicle> vehicles, List<QuoteAutoDriver> drivers, List<QuoteAutoIncident> incidents, QuoteAuto auto)
        {
            if (model == null)
            {
                model = new WatercraftFormSaveModel();
            }
            model.CommonReverseMap(quote, contact, incidents, null);
            if (vehicles != null && vehicles.Any())
            {
                string prefix = "Vehicle";
                foreach (QuoteWatercraftVehicle vehicle in vehicles)
                {
                    int index = vehicles.IndexOf(vehicle);
                    model.SetPropertyValueByIndex(prefix, "AddressSameAsMailing", index, vehicle.KeptAtResidence);
                    model.SetPropertyValueByIndex(prefix, "Zip", index, vehicle.GaragingZipCode);
                    model.SetPropertyValueByIndex(prefix, "YearPurchased", index, vehicle.PurchaseYear);
                    model.SetPropertyValueByIndex(prefix, "HullMaterials", index, vehicle.HullMaterial);
                    model.SetPropertyValueByIndex(prefix, "MotorCount", index, vehicle.NumberOfMotors);
                    model.SetPropertyValueByIndex(prefix, "Horsepower", index, vehicle.TotalHorsepower);
                    model.SetPropertyValueByIndex(prefix, "PropulsionType", index, vehicle.PropulsionType);
                    model.SetPropertyValueByIndex(prefix, "MaxSpeed", index, vehicle.MaxSpeed);
                    model.SetPropertyValueByIndex(prefix, "Value", index, vehicle.Value);
                    model.SetPropertyValueByIndex(prefix, "Year", index, vehicle.Year);
                    model.SetPropertyValueByIndex(prefix, "Make", index, vehicle.Make);
                    model.SetPropertyValueByIndex(prefix, "Model", index, vehicle.Model);
                    model.SetPropertyValueByIndex(prefix, "TrailerIncluded", index, vehicle.IncludesTrailer);
                    model.SetPropertyValueByIndex(prefix, "TrailerValue", index, vehicle.TrailerValue);
                    model.SetPropertyValueByIndex(prefix, "Style", index, vehicle.BodyStyle);
                    model.SetPropertyValueByIndex(prefix, "Length", index, vehicle.LengthInFeet);
                    model.SetPropertyValueByIndex(prefix, "ComprehensiveDeductible", index, vehicle.ComprehensiveDeductible);
                    model.SetPropertyValueByIndex(prefix, "CollisionDeductible", index, vehicle.CollisionDeductible);
                    model.SetPropertyValueByIndex(prefix, "LiabilityOnlyCoverage", index, vehicle.LiabilityOnly);

                    model.SetPropertyValueByIndex(prefix, "Address", index, vehicle.GaragingStreet);
                    model.SetPropertyValueByIndex(prefix, "City", index, vehicle.GaragingCity);
                    model.SetPropertyValueByIndex(prefix, "State", index, vehicle.GaragingState);
                }

                if (contact != null)
                {
                    model.SetPropertyValueByIndex(prefix, "City", 0, contact.PropertyCity);
                    model.SetPropertyValueByIndex(prefix, "State", 0, contact.PropertyState);
                    model.SetPropertyValueByIndex(prefix, "Address", 0, contact.PropertyStreet);
                    model.SetPropertyValueByIndex(prefix, "Zip", 0, contact.PropertyZipCode);
                }
            }

            if (auto != null)
            {
                model.BodilyInjuryPropertyDamage = auto.BodilyInjury;
                model.MedicalCoverage = auto.MedicalCoverage;
                model.UninsuredBoatersCoverage = auto.UninsuredBodilyInjury;
                model.PolicyRenewalType = auto.CurrentPolicyAction;
            }
            if (drivers != null && drivers.Any())
            {
                QuoteAutoDriver policyHolderDriver = drivers.FirstOrDefault(d => d.Key == 0);
                if (policyHolderDriver != null)
                {
                    model.PolicyHolderFirstName = policyHolderDriver.FirstName;
                    model.PolicyHolderLastName = policyHolderDriver.LastName;
                    model.PolicyHolderGender = policyHolderDriver.Gender;
                    model.PolicyHolderDob = policyHolderDriver.BirthDate;
                    model.PolicyHolderOperatingExperience = policyHolderDriver.ExperienceYears;
                    model.PolicyHolderMaritalStatus = policyHolderDriver.MaritalStatus;
                }

                QuoteAutoDriver cohabitantDriver = drivers.FirstOrDefault(d => d.Key == DefaultSpouseCohabitantDriverKey);
                if (cohabitantDriver != null)
                {
                    model.CohabitantFirstName = cohabitantDriver.FirstName;
                    model.CohabitantLastName = cohabitantDriver.LastName;
                    model.CohabitantGender = cohabitantDriver.Gender;
                    model.CohabitantMaritalStatus = cohabitantDriver.MaritalStatus;
                    model.CohabitantDob = cohabitantDriver.BirthDate;
                    model.CohabitantAgeWhenLicensed = cohabitantDriver.AgeLicensed;
                    model.CohabitantOccupation = cohabitantDriver.Occupation;
                    model.CohabitantEducation = cohabitantDriver.Education;
                    model.CohabitantOperatingExperience = cohabitantDriver.ExperienceYears;
                }
                string driverPrefix = "AdditionalDriver";
                foreach (QuoteAutoDriver driver in drivers.Where(d => new[] { 1, 2, 3, 4 }.Contains(d.Key)))
                {
                    int driverIndex = drivers.IndexOf(driver);
                    model.SetPropertyValueByIndex(driverPrefix, "FirstName", driverIndex, driver.FirstName);
                    model.SetPropertyValueByIndex(driverPrefix, "LastName", driverIndex, driver.LastName);
                    model.SetPropertyValueByIndex(driverPrefix, "Gender", driverIndex, driver.Gender);
                    model.SetPropertyValueByIndex(driverPrefix, "MaritalStatus", driverIndex, driver.MaritalStatus);
                    model.SetPropertyValueByIndex(driverPrefix, "Dob", driverIndex, driver.BirthDate);
                    model.SetPropertyValueByIndex(driverPrefix, "OperatingExperience", driverIndex, driver.AgeLicensed);
                }
            }
        }
    }
}