using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm
{
	public class FloodFormSaveModel : CommonFormSaveModel
	{
		[JsonConverter(typeof(YesNoBooleanConverter))]
		public bool AwareOfFloodLossesOnProperty { get; set; }
		public string FloodFoundation { get; set; }
		public string FloodZone { get; set; }
		public string FloorsInStructure { get; set; }
		public string GarageType { get; set; }
		public string GarageValue { get; set; }
		public string HowManyLossesHaveOccurred { get; set; }
		public string HowMuchMortgageInsuranceRequired { get; set; }
		[JsonConverter(typeof(YesNoBooleanConverter))]
		public bool IsStructureACondominium { get; set; }
        public string PropertyAddress { get; set; }
		public bool? PropertyAddressSameAsMailing { get; set; }
		public string PropertyCity { get; set; }
		public string PropertyOccupancy { get; set; }
		public string PropertyState { get; set; }
		public string PropertyYearBuilt { get; set; }
		public string PropertyZip { get; set; }
		
		public string StructuralValue { get; set; }
		
		public string TotalLivingAreaSqFt { get; set; }
		public string WhatFloorIsYourCondominiumOn { get; set; }
    }
}