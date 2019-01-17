namespace ReportModel.Parameters
{
    using System;

    public class ProductReportParameter : ReportParameterBase
    {
        public ProductReportParameter(DateTime date, string value, string shopId) : base(date, value, shopId)
        {
        }

        public string ProductDetailId { get; set; }

        public double StartingQuantity { get; set; }
    }
}