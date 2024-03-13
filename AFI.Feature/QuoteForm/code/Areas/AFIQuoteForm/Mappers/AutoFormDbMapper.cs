using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using FTData = AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class AutoFormDbMapper
    {
        private static readonly int DefaultSpouseCohabitantDriverKey = 5;

        public static void Map(this QuoteAutoVehicle quote, AutoFormSaveModel model, int quoteKey, int vehicleIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoVehicle();
            }

            quote.Key = vehicleIndex;
            quote.QuoteKey = quoteKey;
            string vehiclePrefix = "Vehicle";
            quote.VehicleUse = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Use", vehicleIndex);
            quote.VIN = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Vin", vehicleIndex);
            quote.Year = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Year", vehicleIndex);
            quote.Make = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Make", vehicleIndex);
            quote.Model = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Model", vehicleIndex);
            quote.BodyStyle = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "BodyType", vehicleIndex);
            quote.MilesOneWay = model.GetPropertyValueByIndex<AutoFormSaveModel, int?>(vehiclePrefix, "MilesOneWay", vehicleIndex);
            quote.AnnualMileage = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "AnnualMileage", vehicleIndex);
            quote.LiabilityOnly = model.GetPropertyValueByIndex<AutoFormSaveModel, bool?>(vehiclePrefix, "LiabilityOnlyCoverage", vehicleIndex) ?? false;
            quote.GaragingStreet = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Address", vehicleIndex);
            quote.GaragingCity = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "City", vehicleIndex);
            quote.GaragingState = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "State", vehicleIndex);
            quote.GaragingZip = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "Zip", vehicleIndex);
            quote.CollisionDeductible = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "CollisionDeductible", vehicleIndex);
            quote.ComprehensiveDeductible = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(vehiclePrefix, "ComprehensiveDeductible", vehicleIndex);
        }

        public static void MapPolicyHolder(this QuoteAutoDriver quote, AutoFormSaveModel model, int quoteKey)
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
            quote.AgeLicensed = model.PolicyHolderAgeWhenLicensed;
            quote.GoodStudentDiscount = model.PolicyHolderGoodStudent;
            quote.Occupation = model.PolicyHolderOccupation;
            quote.Education = model.PolicyHolderEducation;
        }

        public static void MapCohabitant(this QuoteAutoDriver quote, AutoFormSaveModel model, int quoteKey)
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
            quote.GoodStudentDiscount = model.CohabitantGoodStudent;
            quote.Occupation = model.CohabitantOccupation;
            quote.Education = model.CohabitantEducation;
        }

        public static void MapSpouse(this QuoteAutoDriver quote, AutoFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = DefaultSpouseCohabitantDriverKey;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.SpouseFirstName;
            quote.LastName = model.SpouseLastName;
            quote.Gender = model.SpouseGender;
            quote.BirthDate = model.SpouseDob;
            quote.Occupation = model.SpouseOccupation;
            quote.Education = model.SpouseEducation;
        }

        public static void MapDriver(this QuoteAutoDriver quote, AutoFormSaveModel model, int quoteKey, int driverIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = driverIndex + 1;
            quote.QuoteKey = quoteKey;
            string driverPrefix = "AdditionalDriver";
            quote.FirstName = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "FirstName", driverIndex);
            quote.LastName = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "LastName", driverIndex);
            quote.Gender = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "Gender", driverIndex);
            quote.MaritalStatus = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "MaritalStatus", driverIndex);
            quote.BirthDate = model.GetPropertyValueByIndex<AutoFormSaveModel, DateTime?>(driverPrefix, "Dob", driverIndex);
            quote.AgeLicensed = model.GetPropertyValueByIndex<AutoFormSaveModel, int?>(driverPrefix, "AgeWhenLicensed", driverIndex);
            quote.GoodStudentDiscount = model.GetPropertyValueByIndex<AutoFormSaveModel, bool>(driverPrefix, "GoodStudent", driverIndex);
            quote.Occupation = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "Occupation", driverIndex);
            quote.Education = model.GetPropertyValueByIndex<AutoFormSaveModel, string>(driverPrefix, "Education", driverIndex);
        }

        public static void ReverseMap(this AutoFormSaveModel model, FTData.Quote quote, QuoteContact contact, List<QuoteAutoVehicle> autoVehicles, List<QuoteAutoDriver> drivers, List<QuoteAutoIncident> incidents, QuoteAuto auto)
        {
            if (model == null)
            {
                model = new AutoFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, incidents, auto);

            if (autoVehicles != null && autoVehicles.Any())
            {
                string vehiclePrefix = "Vehicle";
                for (int index = 0; index < autoVehicles.Count; index++)
                {
                    QuoteAutoVehicle vehicle = autoVehicles[index];
                    model.SetPropertyValueByIndex(vehiclePrefix, "Vin", index, vehicle.VIN);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Year", index, vehicle.Year);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Make", index, vehicle.Make);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Model", index, vehicle.Model);
                    model.SetPropertyValueByIndex(vehiclePrefix, "BodyType", index, vehicle.BodyStyle);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Use", index, vehicle.VehicleUse);
                    model.SetPropertyValueByIndex(vehiclePrefix, "MilesOneWay", index, vehicle.MilesOneWay);
                    model.SetPropertyValueByIndex(vehiclePrefix, "AnnualMileage", index, vehicle.AnnualMileage);
                    model.SetPropertyValueByIndex(vehiclePrefix, "LiabilityOnlyCoverage", index, vehicle.LiabilityOnly);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Address", index, vehicle.GaragingStreet);
                    model.SetPropertyValueByIndex(vehiclePrefix, "City", index, vehicle.GaragingCity);
                    model.SetPropertyValueByIndex(vehiclePrefix, "State", index, vehicle.GaragingState);
                    model.SetPropertyValueByIndex(vehiclePrefix, "Zip", index, vehicle.GaragingZip);
                    model.SetPropertyValueByIndex(vehiclePrefix, "CollisionDeductible", index, vehicle.CollisionDeductible);
                    model.SetPropertyValueByIndex(vehiclePrefix, "ComprehensiveDeductible", index, vehicle.ComprehensiveDeductible);
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
                    model.PolicyHolderMaritalStatus = policyHolderDriver.MaritalStatus;
                    model.PolicyHolderDob = policyHolderDriver.BirthDate;
                    model.PolicyHolderAgeWhenLicensed = policyHolderDriver.AgeLicensed;
                    model.PolicyHolderGoodStudent = policyHolderDriver.GoodStudentDiscount;
                    model.PolicyHolderOccupation = policyHolderDriver.Occupation;
                    model.PolicyHolderEducation = policyHolderDriver.Education;
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
                    model.SetPropertyValueByIndex(driverPrefix, "AgeWhenLicensed", driverIndex, driver.AgeLicensed);
                    model.SetPropertyValueByIndex(driverPrefix, "GoodStudent", driverIndex, driver.GoodStudentDiscount);
                    model.SetPropertyValueByIndex(driverPrefix, "Occupation", driverIndex, driver.Occupation);
                    model.SetPropertyValueByIndex(driverPrefix, "Education", driverIndex, driver.Education);
                }
                QuoteAutoDriver cohabitantSpouseDriver = drivers.FirstOrDefault(d => d.Key == DefaultSpouseCohabitantDriverKey);
                if (cohabitantSpouseDriver != null)
                {
                    model.CohabitantFirstName = cohabitantSpouseDriver.FirstName;
                    model.CohabitantLastName = cohabitantSpouseDriver.LastName;
                    model.CohabitantGender = cohabitantSpouseDriver.Gender;
                    model.CohabitantMaritalStatus = cohabitantSpouseDriver.MaritalStatus;
                    model.CohabitantDob = cohabitantSpouseDriver.BirthDate;
                    model.CohabitantAgeWhenLicensed = cohabitantSpouseDriver.AgeLicensed;
                    model.CohabitantGoodStudent = cohabitantSpouseDriver.GoodStudentDiscount;
                    model.CohabitantOccupation = cohabitantSpouseDriver.Occupation;
                    model.CohabitantEducation = cohabitantSpouseDriver.Education;
                }
            }
        }
    }
}