using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.ShortForms.Models
{
    public class ShortFormViewModel
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }

		public List<Options> States { get; } = CommonData.GetStateList();

		public bool IsEmpty() {
			return string.IsNullOrWhiteSpace(FirstName)
				&& string.IsNullOrWhiteSpace(LastName)
				&& string.IsNullOrWhiteSpace(Email)
				&& string.IsNullOrWhiteSpace(Phone)
				&& string.IsNullOrWhiteSpace(Address)
				&& string.IsNullOrWhiteSpace(City)
				&& string.IsNullOrWhiteSpace(State)
				&& string.IsNullOrWhiteSpace(Zip);
		} 
    }
}