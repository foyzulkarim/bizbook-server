namespace ViewModel.Sales
{
    using System;
    using CommonLibrary.ViewModel;
    using Model.Sales;

    public class InstallmentDetailViewModel : BaseViewModel<InstallmentDetail>
    {
        public InstallmentDetailViewModel(InstallmentDetail x)
            : base(x)
        {
            this.ShopId = x.ShopId;
            this.SaleId = x.SaleId;
            this.InstallmentId = x.InstallmentId;
            this.ScheduledAmount = x.ScheduledAmount;
            this.PaidAmount = x.PaidAmount;
            this.DueAmount = x.DueAmount;
            this.ScheduledDate = x.ScheduledDate;
            this.PaidDate = x.PaidDate;
        }

        public string ShopId { get; set; }

        public string SaleId { get; set; }
        public string InstallmentId { get; set; }
        public double ScheduledAmount { get; set; }

        public double PaidAmount { get; set; }

        public double DueAmount { get; set; }

        public DateTime ScheduledDate { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}