using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using FTData =AFI.Feature.Data.DataModels;
namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class AFIReportView
    {
        public ReportView ReportView { get; set; }
        public QuoteSpouse QuoteSpouse { get; set; }
        public FTData.Quote Quote { get; set; }
        public QuoteAuto QuoteAuto { get; set; }
        public QuoteContact QuoteContact { get; set; }
        public QuoteAutoDriver QuoteAutoDriver { get; set; }
        public QuoteAutoVehicle QuoteAutoVehicle { get; set; }
        public QuoteAutoIncident QuoteAutoIncident { get; set; }
        public QuoteCommercial QuoteCommercial { get; set; }
        public QuoteFlood QuoteFlood { get; set; }
        public QuoteHomeowner QuoteHomeowner { get; set; }
        public QuoteHomeownerLoss QuoteHomeownerLoss { get; set; }
        public QuoteMotorcycleVehicle QuoteMotorcycleVehicle { get; set; }
        public IEnumerable<FTData.Quote> quotes { get; set; }
        public IEnumerable<QuoteContact> quotesStep { get; set; }
        public IEnumerable<QuoteAutoVehicle> vehicles { get; set; }
        public IEnumerable<QuoteAutoDriver> drivers { get; set; }
        public IEnumerable<QuoteContact> contacts { get; set; }
        public IEnumerable<QuoteAuto> autos { get; set; }
        public IEnumerable<QuoteAutoIncident> incidents { get; set; }
        public IEnumerable<QuoteCommercial> commercials { get; set; }
        public IEnumerable<QuoteCommercial> commercialsStep { get; set; }
        public IEnumerable<QuoteFlood> floods { get; set; }
        public IEnumerable<QuoteHomeowner> homeowners { get; set; }
        public IEnumerable<QuoteHomeownerLoss> homeownerLoss { get; set; }
        public IEnumerable<QuoteMotorcycleVehicle>motorcycles { get; set; }
        public int vehiclesCount { get; set; }
        public int quotesCount { get; set; }
        public int quoteStepCount { get; set; }
        public int driversCount { get; set; }
        public int contactsCount { get; set; }
        public int autosCount { get; set; }
        public int incidentsCount { get; set; }
        public int CommercialsCount { get; set; }
        public int CommercialStepCount { get; set; }
        public int FloodCount { get; set; }
        public int homeownerCount { get; set; }
        public int motorcycleCount { get; set; }
        public IEnumerable<ReportView> UpdatedReports { get; set; }
        public IEnumerable<ReportView> SectionPerformances { get; set; }
        public IEnumerable<ReportView> AutoSectionPerformances { get; set; }
        public ReportView CoverageInfo { get; set; }
        public QHReport QuotehubReport { get; set; }
        public IEnumerable<QHReport> QuotehubReports { get; set; }
        public IEnumerable<QHReport> QHActivityReports { get; set; }
        public IEnumerable<QHCSVReport> QHCSVReports { get; set; }
        public IEnumerable<VoteReport> VoteReports { get; set; }
    }
}