namespace ViewModel.Reports
{
    using Model;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class SaleReportGridModel
    {
        public SaleReportGridModel(SaleReportViewModel report)
        {
            // SaleReportType = report.SaleReportType;
            this.Value = report.Value;
            this.SaleFrom = report.SaleFrom;
            this.SaleChannel = report.SaleChannel;
            this.TotalAmount = report.TotalAmount;
            this.DiscountAmount = report.DiscountAmount;
            this.PayableTotalAmount = report.PayableTotalAmount;
            this.PaidAmount = report.PaidAmount;
            this.DueAmount = report.DueAmount;
        }

        public string Value { get; set; }        

        [JsonConverter(typeof(StringEnumConverter))]
        public SaleFrom SaleFrom { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SaleChannel SaleChannel { get; set; }

        public double TotalAmount { get; set; }

        public double DiscountAmount { get; set; }

        public double PayableTotalAmount { get; set; }

        public double PaidAmount { get; set; }

        public double DueAmount { get; set; }
    }
}