using System;

namespace ViewModel.Purchases
{
    public class PurchaseReportModel
    {
        public string Memo { get; set; }
        public string Supplier { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string ModifiedBy { get; set; }
    }
}