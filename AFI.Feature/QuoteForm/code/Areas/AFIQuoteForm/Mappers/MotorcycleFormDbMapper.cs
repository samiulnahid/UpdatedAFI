using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class MotorcycleFormDbMapper
    {
        private static readonly int DefaultSpouseCohabitantDriverKey = 5;
        public static void Map(this QuoteMotorcycleVehicle quote, MotorcycleFormSaveModel model, int quoteKey, int vehicleIndex)
        {
            if (quote == null)
            {
                quote = new QuoteMotorcycleVehicle();
            }

            quote.Key = vehicleIndex;
            quote.QuoteKey = quoteKey;
            string prefix = "Vehicle";
            quote.PurchaseYear = "2000"; //TODO: This is 2000 because this field is required on the DB (not null) but is not requested in the form.
            quote.Year = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, int>(prefix, "Year", vehicleIndex);
            quote.Make = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "Make", vehicleIndex);
            quote.Model = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "Model", vehicleIndex);
            quote.VehicleType = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "Type", vehicleIndex);
            quote.Value = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, int?>(prefix, "Value", vehicleIndex);
            quote.IsLiabilityOnly = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, bool>(prefix, "LiabilityOnlyCoverage", vehicleIndex);
            quote.CCSize = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "CcSize", vehicleIndex);
            quote.CollisionDeductible = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "CollisionDeductible", vehicleIndex);
            quote.ComprehensiveDeductible = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "ComprehensiveDeductible", vehicleIndex);
            quote.GaragingStreet = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "Address", vehicleIndex);
            quote.GaragingCity = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "City", vehicleIndex);
            quote.GaragingState = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "State", vehicleIndex);
            quote.GaragingZip = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(prefix, "Zip", vehicleIndex);
        }

        public static void MapCohabitant(this QuoteAutoDriver quote, MotorcycleFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = DefaultSpouseCohabitantDriverKey;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.CohabitantFirstName;
            quote.LastName = model.CohabitantLastName;
            quote.Gender = model.CohabitantGender;
            quote.MaritalStatus = model.CohabitantMaritalStatus;
            quote.BirthDate = model.CohabitantDob;
            quote.AgeLicensed = model.CohabitantAgeWhenLicensed;
            quote.Occupation = model.CohabitantOccupation;
            quote.Education = model.CohabitantEducation;
            quote.ExperienceYears = model.CohabitantYearsExperience;


        }

        public static void MapPolicyHolder(this QuoteAutoDriver quote, MotorcycleFormSaveModel model, int quoteKey)
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
            quote.BirthDate = model.PolicyHolderDob;
            quote.AgeLicensed = model.PolicyHolderAgeWhenLicensed;
            quote.SSN = model.PolicyHolderSsn;
            quote.ExperienceYears = model.PolicyHolderOperatingExperience;
        }

        public static void MapDriver(this QuoteAutoDriver quote, MotorcycleFormSaveModel model, int quoteKey, int driverIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = driverIndex + 1;
            quote.QuoteKey = quoteKey;
            string driverPrefix = "AdditionalDriver";
            quote.FirstName = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(driverPrefix, "FirstName", driverIndex);
            quote.LastName = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(driverPrefix, "LastName", driverIndex);
            quote.SSN = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(driverPrefix, "Ssn", driverIndex);
            quote.BirthDate = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, DateTime?>(driverPrefix, "Dob", driverIndex);
            quote.Gender = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>(driverPrefix, "Gender", driverIndex);
            quote.ExperienceYears = model.GetPropertyValueByIndex<MotorcycleFormSaveModel, int?>(driverPrefix, "YearsExperience", driverIndex);
        }

        public static void ReverseMap(this MotorcycleFormSaveModel model, FTData.Quote quote, QuoteContact contact, List<QuoteMotorcycleVehicle> motorcycle, List<QuoteAutoDriver> drivers, List<QuoteAutoIncident> incidents, QuoteAuto auto)
        {
            if (model == null)
            {
                model = new MotorcycleFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, incidents, auto);

            if (motorcycle != null && motorcycle.Any())
            {
                string vehiclePrefix = "Vehicle";
                foreach (QuoteMotorcycleVehicle vehicle in motorcycle)
                {
                    int vehicleIndex = motorcycle.IndexOf(vehicle);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Year", vehicleIndex, vehicle.Year);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Make", vehicleIndex, vehicle.Make);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Model", vehicleIndex, vehicle.Model);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Type", vehicleIndex, vehicle.VehicleType);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Value", vehicleIndex, vehicle.Value);
                    model.SetPropertyValueByIndex(vehiclePrefix, "LiabilityOnlyCoverage", vehicleIndex, vehicle.IsLiabilityOnly);
                    model.SetPropertyValueByIndex(vehiclePrefix, "CcSize", vehicleIndex, vehicle.CCSize);
                    model.SetPropertyValueByIndex(vehiclePrefix, "CollisionDeductible", vehicleIndex, vehicle.CollisionDeductible);
                    model.SetPropertyValueByIndex(vehiclePrefix, "ComprehensiveDeductible", vehicleIndex, vehicle.ComprehensiveDeductible);


                    model.SetPropertyValueByIndex(vehiclePrefix, "Address", vehicleIndex, vehicle.GaragingStreet);
                    model.SetPropertyValueByIndex(vehiclePrefix, "City", vehicleIndex, vehicle.GaragingCity);
                    model.SetPropertyValueByIndex(vehiclePrefix, "State", vehicleIndex, vehicle.GaragingState);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Zip", vehicleIndex, vehicle.GaragingZip);
                }

                if (contact != null)
                {
                    model.SetPropertyValueByIndex(vehiclePrefix, "City", 0, contact.PropertyCity);
                    model.SetPropertyValueByIndex(vehiclePrefix, "State", 0, contact.PropertyState);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Address", 0, contact.PropertyStreet);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Zip", 0, contact.PropertyZipCode);
                }
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
                    model.PolicyHolderSsn = policyHolderDriver.SSN;
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
                    model.CohabitantYearsExperience = cohabitantDriver.ExperienceYears;
                }

                string driverPrefix = "AdditionalDriver";
                foreach (QuoteAutoDriver driver in drivers.Where(d => new[] { 1, 2, 3, 4 }.Contains(d.Key)))
                {
                    int driverIndex = drivers.IndexOf(driver);
                    model.SetPropertyValueByIndex(driverPrefix, "FirstName", driverIndex, driver.FirstName);
                    model.SetPropertyValueByIndex(driverPrefix, "LastName", driverIndex, driver.LastName);
                    model.SetPropertyValueByIndex(driverPrefix, "Gender", driverIndex, driver.Gender);
                    model.SetPropertyValueByIndex(driverPrefix, "Dob", driverIndex, driver.BirthDate);
                    model.SetPropertyValueByIndex(driverPrefix, "YearsExperience", driverIndex, driver.ExperienceYears);
                    model.SetPropertyValueByIndex(driverPrefix, "Ssn", driverIndex, driver.SSN);
                }
            }
        }
    }
}