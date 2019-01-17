namespace ReportModel.Parameters
{
    using System;

    public class SaleReportParameter : ReportParameterBase
    {
        public SaleReportParameter(DateTime date, string value, string shopId) : base(date, value, shopId)
        {
        }

        public SaleType SaleType { get; set; }
    }

    public enum SaleType
    {
        All = 0,
        DealerSale = 1,
        CustomerSale = 2
    }
}