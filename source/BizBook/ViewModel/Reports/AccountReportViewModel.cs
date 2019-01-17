namespace ViewModel.Reports
{
    using Model;
    using ReportModel;

    public class AccountReportViewModel : BaseReportViewModel<AccountReport>
    {
        public AccountReportViewModel(AccountReport x)
            : base(x)
        {
            this.AccountHeadName = x.AccountHeadName;
            AmountTotalStarting = x.AmountTotalStarting;
            AmountTotalIn = x.AmountTotalIn;
            AmountTotalOut = x.AmountTotalOut;
            AmountTotalEnding = x.AmountTotalEnding;
        }

        public AccountReportType TransactionReportType { get; set; }

        public string AccountHeadName { get; set; }


        public double AmountTotalStarting { get; set; } = 0;


        public double AmountTotalIn { get; set; } = 0;


        public double AmountTotalOut { get; set; } = 0;


        public double AmountTotalEnding { get; set; } = 0;


        public int CountTotalTrx { get; set; } = 0;
    }
}