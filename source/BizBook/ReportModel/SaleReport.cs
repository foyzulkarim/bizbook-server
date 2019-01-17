namespace ReportModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ReportModel.Parameters;

    public class SaleReport : BaseReport
    {
        #region Classifications

        [Index] [Required] public SaleType SaleType { get; set; } = SaleType.All;

        #endregion

        #region Amount

        [Index]
        [Required]
        [DataType(DataType.Currency)]
        public double AmountProduct { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountDeliveryCharge { get; set; } = 0;

        [DataType(DataType.Currency)] public double AmountTax { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountOther { get; set; } = 0;

        [Index]
        [Required]
        [DataType(DataType.Currency)]
        public double AmountTotal { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountDiscount { get; set; } = 0;

        [Index]
        [Required]
        [DataType(DataType.Currency)]
        public double AmountPayable { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPaid { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountDue { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountCost { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfit { get; set; } = 0; // payabletotal - cost

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfitPercent { get; set; } = 0; // (profitamount * 100) / costamount

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfitPercentInAllSale { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPreviousDueCollection { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountFromInhouse { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountFromWebsite { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountFromFacebook { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountFromOther { get; set; } = 0;

        #endregion

        #region Count

        [Required] public int CountSaleCreated { get; set; } = 0;

        [Required] public int CountSaleCompleted { get; set; } = 0;

        #endregion
    }
}