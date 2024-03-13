namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
	public class PaginationViewModel
	{
		public string RawUrl { get; set; }
		public int TotalResults { get; set; }
		public int PageSize { get; set; }
		public int Page { get; set; }
	}
}