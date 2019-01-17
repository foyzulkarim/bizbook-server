namespace ReportModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductReport : BaseReport
    {
        #region Classifications

        [Required] [Index] [MaxLength(128)] public string ProductGroupId { get; set; }

        [Required]
        [Index]
        [MaxLength(128)]
        [DataType(DataType.Text)]
        public string ProductGroupName { get; set; }

        [Required] [Index] [MaxLength(128)] public string ProductCategoryId { get; set; }

        [Required]
        [Index]
        [MaxLength(128)]
        [DataType(DataType.Text)]
        public string ProductCategoryName { get; set; }

        [Required] [Index] [MaxLength(128)] public string ProductDetailId { get; set; }

        [Required]
        [Index]
        [MaxLength(128)]
        [DataType(DataType.Text)]
        public string ProductDetailName { get; set; }

        #endregion

        #region Quantities

        [Required] public double QuantityStartingToday { get; set; } = 0;

        [Required] public double QuantityEndingToday { get; set; } = 0;

        [Required] public double QuantityPurchaseToday { get; set; } = 0;

        [Required] public double QuantityPurchasePercentInAllProductsToday { get; set; } = 0;

        [Required] public double QuantitySaleToday { get; set; } = 0;

        public double QuantitySalePendingToday { get; set; } = 0;

        public double QuantitySaleProcessingToday { get; set; } = 0;

        public double QuantitySaleDoneToday { get; set; } = 0;

        public double QuantityStockInApprovedToday { get; set; } = 0;

        public double QuantityStockOutApprovedToday { get; set; } = 0;

        public double QuantityStockInPendingToday { get; set; } = 0;

        public double QuantityStockOutPendingToday { get; set; } = 0;

        [Required] public double QuantitySalePercentInAllProductsToday { get; set; } = 0;

        [Required] public double QuantitySaleToDealerToday { get; set; } = 0;

        [Required] public double QuantitySaleToCustomerToday { get; set; } = 0;

        #endregion

        #region Amounts Sale

        [Required]
        [DataType(DataType.Currency)]
        public double AmountSaleToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountCostForSaleToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountSalePercentInAllProductsToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountReceivedToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountReceivableToday { get; set; } = 0; // Due

        [Required]
        [DataType(DataType.Currency)]
        public double AmountAverageSalePriceToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfitToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfitPercentToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountProfitPercentInAllProductsToday { get; set; } = 0;

        [Required] public double AmountSaleToDealerToday { get; set; } = 0;

        [Required] public double AmountSaleToCustomerToday { get; set; } = 0;

        #endregion

        #region Amounts Purchase

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPurchaseToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPurchasePercentInAllProductsToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPaidToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountPayableToday { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountAveragePurchasePricePerUnitToday { get; set; } = 0;

        #endregion
    }

    public class StockReportModelTemp
    {
        public string ProductDetailId { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public int RowsCount { get; set; }
    }
}