using System.Collections.Generic;

namespace Model.Sales
{
    public class Installment : ShopChild
    {
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

        public string SaleId { get; set; }

        public virtual ICollection<InstallmentDetail> InstallmentDetails { get; set; }
    }
}