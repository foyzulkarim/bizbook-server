namespace ReportModel.Parameters
{
    using System;

    public class TransactionReportParameter : ReportParameterBase
    {
        public TransactionReportParameter(DateTime date, string value, string shopId) : base(date, value, shopId)
        {
        }

        public string AccountHeadName { get; set; }

        public string AccountHeadId { get; set; }

        public double TotalStarting { get; set; }

        public double CashStarting { get; set; }

        public double CardStarting { get; set; }

        public double ChequeStarting { get; set; }

        public double MobileStarting { get; set; }

        public double OtherStarting { get; set; }
    }
}