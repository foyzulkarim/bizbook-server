using System.Collections.Generic;
using System.Linq;

namespace ViewModel.Sales
{
    using CommonLibrary.ViewModel;
    using Model.Sales;

    public class InstallmentViewModel : BaseViewModel<Installment>
    {
        public InstallmentViewModel(Installment x)
            : base(x)
        {
            //if (x.Sale!=null)
            //{
            //    // do loading of sale
            //    //Sale = new SaleViewModel(x.Sale);
            //}
            if (x.InstallmentDetails != null)
            {
                InstallmentDetails = x.InstallmentDetails.ToList().ConvertAll(y => new InstallmentDetailViewModel(y))
                    .ToList();
            }

            CashPriceAmount = x.CashPriceAmount;
            ProfitPercent = x.ProfitPercent;
            ProfitAmount = x.ProfitAmount;
            InstallmentTotalAmount = x.InstallmentTotalAmount;
            DownPaymentPercent = x.DownPaymentPercent;
            DownPaymentAmount = x.DownPaymentAmount;
            InstallmentDueAmount = x.InstallmentDueAmount;

            InstallmentMonth = x.InstallmentMonth;
            InstallmentPerMonthAmount = x.InstallmentPerMonthAmount;

            SaleType = x.SaleType;
            CashPriceDueAmount = x.CashPriceDueAmount;

            ProfitAmountPerMonth = x.ProfitAmountPerMonth;
        }

        public double CashPriceAmount { get; set; }

        public double ProfitPercent { get; set; }

        public double ProfitAmount { get; set; }

        public double InstallmentTotalAmount { get; set; }

        public double DownPaymentPercent { get; set; }

        public double DownPaymentAmount { get; set; }

        public double InstallmentDueAmount { get; set; }

        public string InstallmentMonth { get; set; }

        public double InstallmentPerMonthAmount { get; set; }

        public int SaleType { get; set; }

        public double CashPriceDueAmount { get; set; }

        public double ProfitAmountPerMonth { get; set; }

        public List<InstallmentDetailViewModel> InstallmentDetails { get; set; }

        public SaleViewModel Sale { get; set; }
    }
}