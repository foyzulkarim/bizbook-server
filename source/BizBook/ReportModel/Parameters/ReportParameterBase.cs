namespace ReportModel.Parameters
{
    using System;

    public class ReportParameterBase
    {
        public ReportParameterBase(DateTime date, string value, string shopId)
        {
            this.Date = date;
            this.Value = value;
            this.ReportShopId = shopId;
        }

        public DateTime Date { get; }
        public string Value { get; }
        public string ReportShopId { get; }
    }
}