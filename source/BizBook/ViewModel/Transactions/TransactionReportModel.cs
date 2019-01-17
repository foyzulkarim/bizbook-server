namespace ViewModel.Transactions
{
    using System;

    public class TransactionReportModel
    {
        public string Type { get; set; }
        public string Order { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public string Service { get; set; }
    }
}