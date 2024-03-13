using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Feature.WebQuoteService.Models
{
	public class PartnerAdvisor
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleInitial { get; set; }
	}

	public class PartnerAdvisorEqualityComparer : IEqualityComparer<PartnerAdvisor>
	{
		public bool Equals(PartnerAdvisor x, PartnerAdvisor y)
		{
			return string.Equals(x.FirstName, y.FirstName, StringComparison.InvariantCultureIgnoreCase)
				&& string.Equals(x.MiddleInitial, y.MiddleInitial, StringComparison.InvariantCultureIgnoreCase)
				&& string.Equals(x.LastName, y.LastName, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(PartnerAdvisor obj)
		{
			string code = obj.FirstName + obj.MiddleInitial + obj.LastName;
			return code.GetHashCode();
		}
	}
}
