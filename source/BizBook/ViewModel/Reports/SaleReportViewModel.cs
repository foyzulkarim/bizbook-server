namespace ViewModel.Reports
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using ReportModel;
    using ReportModel.Parameters;

    public class SaleReportViewModel : BaseReportViewModel<SaleReport>
    {
        public SaleReportViewModel(SaleReport report) : base(report)
        {
            SaleType = report.SaleType;
            AmountProduct = report.AmountProduct;
            AmountDiscount = report.AmountDiscount;
            AmountTotal = report.AmountTotal;
            AmountPayable = report.AmountPayable;
            AmountPaid = report.AmountPaid;
            AmountDue = report.AmountDue;
            AmountCost = report.AmountCost;
            AmountProfitPercent = report.AmountProfitPercent;
            AmountCost = report.AmountCost;
            AmountProfit = report.AmountProfit;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public SaleType SaleType { get; set; }

        public double AmountProduct { get; set; } = 0;
        public double AmountDeliveryCharge { get; set; } = 0;
        public double AmountTax { get; set; } = 0;
        public double AmountOther { get; set; } = 0;
        public double AmountTotal { get; set; } = 0;
        public double AmountDiscount { get; set; } = 0;
        public double AmountPayable { get; set; } = 0;
        public double AmountPaid { get; set; } = 0;
        public double AmountDue { get; set; } = 0;
        public double AmountCost { get; set; } = 0;
        public double AmountProfit { get; set; } = 0; // payabletotal - cost
        public double AmountProfitPercent { get; set; } = 0; // (profitamount * 100) / costamount
        public double AmountProfitPercentInAllSale { get; set; } = 0;
        public double AmountPreviousDueCollection { get; set; } = 0;
        public double AmountFromInhouse { get; set; } = 0;
        public double AmountFromWebsite { get; set; } = 0;
        public double AmountFromFacebook { get; set; } = 0;
        public double AmountFromOther { get; set; } = 0;
        public int CountSaleCreated { get; set; } = 0;
        public int CountSaleCompleted { get; set; } = 0;
    }
}