using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
	public class ProcessorResponseModel
	{
		public bool WasSuccessful { get; set; }
		public string FailureMessage { get; set; }
	}
}