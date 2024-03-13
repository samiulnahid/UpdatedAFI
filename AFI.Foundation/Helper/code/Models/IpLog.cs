using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Foundation.Helper.Models
{
    public class IpLog
    {
        public long Id { get; set; }
        public string IP { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTime AddedDate { get; set; }

        [NotMapped]
        public string TotalCount { get; set; }
        [NotMapped]
        public string FormattedAddedDate
        {
            get
            {
                // Parse the Sitecore DateTime string
                var sDate = Sitecore.DateUtil.GetShortIsoDateTime(AddedDate);
                DateTime dateTime = DateTime.ParseExact(sDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                // Return the formatted date
                return dateTime.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
               
            }
        }
    }
}
