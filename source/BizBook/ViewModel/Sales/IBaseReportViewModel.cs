namespace ViewModel.Sales
{
    public interface IBaseReportViewModel
    {
        string DateRange { get; set; }
        int EntryFound { get; set; }
    }
}