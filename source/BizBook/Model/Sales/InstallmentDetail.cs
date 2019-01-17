namespace Model.Sales
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Model.Transactions;

    public class InstallmentDetail : ShopChild
    {
        public string InstallmentId { get; set; }

        [ForeignKey("InstallmentId")] public virtual Installment Installment { get; set; }

        public string SaleId { get; set; }

        [ForeignKey("SaleId")] public virtual Sale Sale { get; set; }

        public string TansactionId { get; set; }

        [ForeignKey("TansactionId")] public virtual Transaction Transaction { get; set; }

        public double ScheduledAmount { get; set; }

        public double PaidAmount { get; set; }

        public double DueAmount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ScheduledDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PaidDate { get; set; }
    }
}