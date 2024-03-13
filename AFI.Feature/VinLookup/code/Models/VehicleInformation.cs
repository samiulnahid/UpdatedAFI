using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Feature.VinLookup.Models
{
	public class VehicleInformation
	{
		public int Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string BodyStyle { get; set; }
		public string Vin { get; set; }
	}
}
