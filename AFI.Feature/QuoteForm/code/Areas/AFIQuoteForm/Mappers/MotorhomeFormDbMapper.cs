using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class MotorhomeFormDbMapper
    {

        public static void ReverseMap(this MotorhomeFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteMotorhomeVehicle vehicle, List<QuoteAutoDriver> drivers, List<QuoteAutoIncident> incidents, QuoteAuto auto, QuoteAutoVehicle autoVehicle)
        {
            if (model == null)
            {
                model = new MotorhomeFormSaveModel();
            }
            model.CommonReverseMap(quote, contact, incidents, auto);
            if (auto != null)
            {
                model.VehicleComprehensiveDeductible = auto.ComprehensiveDeductible;
                model.VehicleCollisionDeductible = auto.CollisionDeductible;
                model.PolicyRenewalType = auto.CurrentPolicyAction;
            }
            if (vehicle != null)
            {
                model.VehiclePurchaseYear = vehicle.PurchaseYear;
                model.VehicleValue = vehicle.Value;
                model.VehicleMake = vehicle.Make;
                model.VehicleType = vehicle.VehicleType;
                model.VehicleYear = vehicle.Year;
                model.VehicleModel = vehicle.Model;
                model.VehicleUse = vehicle.Usage;
                model.VehiclePersonalEffectsValue = vehicle.ValuePersonalEffects;
                model.VehicleLength = vehicle.Length;
                model.VehicleSlideOutCount = vehicle.NumberOfSlideOuts;
            }

            if (autoVehicle != null)
            {
                model.VehicleLiabilityOnlyCoverage = autoVehicle.LiabilityOnly ?? false;
                model.VehicleComprehensiveDeductible = autoVehicle.ComprehensiveDeductible;
                model.VehicleCollisionDeductible = autoVehicle.CollisionDeductible;
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
                    model.PolicyHolderAgeWhenLicensed = policyHolderDriver.AgeLicensed;
                }

                QuoteAutoDriver primaryDriver = drivers.FirstOrDefault(d => d.Key == 1);
                if (primaryDriver != null)
                {
                    model.PrimaryDriverFirstName = primaryDriver.FirstName;
                    model.PrimaryDriverLastName = primaryDriver.LastName;
                    model.PrimaryDriverDob = primaryDriver.BirthDate;
                    model.PrimaryDriverGender = primaryDriver.Gender;
                    model.PrimaryDriverMaritalStatus = primaryDriver.MaritalStatus;
                }

                bool isMarriedOrSpouse = string.Equals(quote.Eligibility, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase) ||
                                         string.Equals(contact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) ||
                                         string.Equals(contact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase);
                int[] driverKeys = isMarriedOrSpouse ? new[] {3, 4} : new[] {2, 3, 4};
                if (isMarriedOrSpouse)
                {
                    QuoteAutoDriver spouseDriver = drivers.FirstOrDefault(d => d.Key == 2);
                    if (spouseDriver != null)
                    {
                        model.SpouseOccupation = spouseDriver.Occupation;
                        model.SpouseEducation = spouseDriver.Education;
                        model.SpouseAgeWhenLicensed = spouseDriver.AgeLicensed;
                    }

                }

                string driverPrefix = "AdditionalDriver";
                foreach (QuoteAutoDriver driver in drivers.Where(d => driverKeys.Contains(d.Key)))
                {
                    int driverIndex = drivers.IndexOf(driver);
                    model.SetPropertyValueByIndex(driverPrefix, "FirstName", driverIndex, driver.FirstName);
                    model.SetPropertyValueByIndex(driverPrefix, "LastName", driverIndex, driver.LastName);
                    model.SetPropertyValueByIndex(driverPrefix, "Ssn", driverIndex, driver.SSN);
                    model.SetPropertyValueByIndex(driverPrefix, "Dob", driverIndex, driver.BirthDate);
                    model.SetPropertyValueByIndex(driverPrefix, "Gender", driverIndex, driver.Gender);
                    model.SetPropertyValueByIndex(driverPrefix, "MaritalStatus", driverIndex, driver.MaritalStatus);
                    model.SetPropertyValueByIndex(driverPrefix, "OperatingExperience", driverIndex, driver.ExperienceYears);
                }
            }
        }

        public static void Map(this QuoteMotorhomeVehicle quote, MotorhomeFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteMotorhomeVehicle();
            }

            quote.Key = 0;
            quote.QuoteKey = quoteKey;
            quote.PurchaseYear = model.VehiclePurchaseYear;
            quote.Value = model.VehicleValue;
            quote.Make = model.VehicleMake;
            quote.VehicleType = model.VehicleType;
            quote.Year = model.VehicleYear;
            quote.Model = model.VehicleModel;
            quote.Usage = model.VehicleUse;
            quote.ValuePersonalEffects = model.VehiclePersonalEffectsValue;
            quote.Length = model.VehicleLength;
            quote.NumberOfSlideOuts = model.VehicleSlideOutCount;
        }

        public static void MapPolicyHolder(this QuoteAutoDriver quote, MotorhomeFormSaveModel model, int quoteKey)
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
            quote.ExperienceYears = model.PolicyHolderOperatingExperience;
            quote.SSN = model.PolicyHolderSsn;
            quote.AgeLicensed = model.PolicyHolderAgeWhenLicensed;
            quote.MaritalStatus = model.PolicyHolderMaritalStatus;
        }

        public static void MapPrimaryDriver(this QuoteAutoDriver quote, MotorhomeFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }
            quote.Key = 1;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.PrimaryDriverFirstName;
            quote.LastName = model.PrimaryDriverLastName;
            quote.Gender = model.PrimaryDriverGender;
            quote.BirthDate = model.PrimaryDriverDob;
            quote.MaritalStatus = model.PrimaryDriverMaritalStatus;
            quote.ExperienceYears = model.PrimaryDriverOperatingExperience;
        }

        public static void MapSpouseDriver(this QuoteAutoDriver quote, MotorhomeFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = 5;
            quote.QuoteKey = quoteKey;
            quote.FirstName = model.SpouseFirstName;
            quote.LastName = model.SpouseLastName;
            quote.SSN = model.SpouseSsn;
            quote.BirthDate = model.SpouseDob;
            quote.Gender = model.SpouseGender;
            quote.Occupation = model.SpouseOccupation;
            quote.Education = model.SpouseEducation;
            quote.AgeLicensed = model.SpouseAgeWhenLicensed;
            quote.ExperienceYears = model.SpouseMotorHomeOperatingExperience;
        }

        public static void MapDriver(this QuoteAutoDriver quote, MotorhomeFormSaveModel model, int quoteKey, int driverKey, int driverIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoDriver();
            }

            quote.Key = driverKey;
            quote.QuoteKey = quoteKey;
            string driverPrefix = "AdditionalDriver";
            quote.FirstName = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, string>(driverPrefix, "FirstName", driverIndex);
            quote.LastName = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, string>(driverPrefix, "LastName", driverIndex);
            quote.BirthDate = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, DateTime?>(driverPrefix, "Dob", driverIndex);
            quote.Gender = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, string>(driverPrefix, "Gender", driverIndex);
            quote.MaritalStatus = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, string>(driverPrefix, "MaritalStatus", driverIndex);
            quote.ExperienceYears = model.GetPropertyValueByIndex<MotorhomeFormSaveModel, int?>(driverPrefix, "OperatingExperience", driverIndex);
        }



        public static void MapVehicle(this QuoteAutoVehicle quote, MotorhomeFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAutoVehicle();
            }

            quote.Key = 0;
            quote.QuoteKey = quoteKey;
            quote.LiabilityOnly = model.VehicleLiabilityOnlyCoverage;
            quote.ComprehensiveDeductible = model.VehicleComprehensiveDeductible;
            quote.CollisionDeductible = model.VehicleCollisionDeductible;
        }

        public static void Map(this QuoteAuto quote, MotorhomeFormSaveModel model, int quoteKey)
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
            quote.NumberOfLicensedDrivers = model.LicensedDriverCount;
            quote.ComprehensiveDeductible = model.VehicleComprehensiveDeductible;
            quote.CollisionDeductible = model.VehicleCollisionDeductible;
            quote.CurrentPolicyAction = model.PolicyRenewalType;
        }
    }
}