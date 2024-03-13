using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorForm
{
    public class CollectorVehicleForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public CollectorVehicleFormData form { get; set; }
    }
}