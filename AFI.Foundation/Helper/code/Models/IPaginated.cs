namespace AFI.Foundation.Helper.Models
{
    public interface IPaginated
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
}
