namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public interface IPaginated
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
}
