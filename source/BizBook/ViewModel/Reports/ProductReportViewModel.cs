namespace ViewModel.Reports
{
    using ReportModel;

    public class ProductReportViewModel : BaseReportViewModel<ProductReport>
    {
        public ProductReportViewModel(ProductReport x)
            : base(x)
        {
            ProductGroupId = x.ProductGroupId;
            ProductGroupName = x.ProductGroupName;
            ProductCategoryId = x.ProductCategoryId;
            ProductCategoryName = x.ProductCategoryName;
            ProductDetailId = x.ProductDetailId;
            ProductDetailName = x.ProductDetailName;

            //Quantities
            QuantityStartingToday = x.QuantityStartingToday;
            QuantityEndingToday = x.QuantityEndingToday;
            QuantityPurchaseToday = x.QuantityPurchaseToday;
            QuantityPurchasePercentInAllProductsToday = x.QuantityPurchasePercentInAllProductsToday;
            QuantitySaleToday = x.QuantitySaleToday;
            QuantitySalePendingToday = x.QuantitySalePendingToday;
            QuantitySaleProcessingToday = x.QuantitySaleProcessingToday;
            QuantitySaleDoneToday = x.QuantitySaleDoneToday;
            QuantitySalePercentInAllProductsToday = x.QuantitySalePercentInAllProductsToday;
            QuantitySaleToDealerToday = x.QuantitySaleToDealerToday;
            QuantitySaleToCustomerToday = x.QuantitySaleToCustomerToday;
            QuantityStockIn = x.QuantityStockInApprovedToday;
            QuantityStockOut =x.QuantityStockOutApprovedToday;
            
            //Amounts Sale

            AmountSaleToday = x.AmountSaleToday;
            AmountCostForSaleToday = x.AmountCostForSaleToday;

            AmountSalePercentInAllProductsToday = x.AmountSalePercentInAllProductsToday;
            AmountReceivedToday = x.AmountReceivedToday;

            AmountReceivableToday = x.AmountReceivableToday;
            AmountAverageSalePriceToday = x.AmountAverageSalePriceToday;

            AmountProfitToday = x.AmountProfitToday;
            AmountProfitPercentToday = x.AmountProfitPercentToday;

            AmountProfitPercentInAllProductsToday = x.AmountProfitPercentInAllProductsToday;

            AmountSaleToDealerToday = x.AmountSaleToDealerToday;

            AmountSaleToCustomerToday = x.AmountSaleToCustomerToday;
            //Amounts Purchase
            AmountPurchaseToday = x.AmountPurchaseToday;

            AmountPurchasePercentInAllProductsToday = x.AmountPurchasePercentInAllProductsToday;
            AmountPaidToday = x.AmountPaidToday;
            AmountPayableToday = x.AmountPayableToday;
            AmountAveragePurchasePricePerUnitToday = x.AmountAveragePurchasePricePerUnitToday;
        }

        public double QuantityStockOut { get; set; }

        public double QuantityStockIn { get; set; }

        #region Classifications

        public string ProductGroupId { get; set; }

        public string ProductGroupName { get; set; }

        public string ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductDetailId { get; set; }

        public string ProductDetailName { get; set; }

        #endregion

        #region Quantities

        public double QuantityStartingToday { get; set; }

        public double QuantityEndingToday { get; set; }


        public double QuantityPurchaseToday { get; set; }


        public double QuantityPurchasePercentInAllProductsToday { get; set; }


        public double QuantitySaleToday { get; set; }

        public double QuantitySalePendingToday { get; set; }

        public double QuantitySaleProcessingToday { get; set; }

        public double QuantitySaleDoneToday { get; set; }

        public double QuantitySalePercentInAllProductsToday { get; set; }


        public double QuantitySaleToDealerToday { get; set; }


        public double QuantitySaleToCustomerToday { get; set; }

        #endregion

        #region Amounts Sale

        public double AmountSaleToday { get; set; }


        public double AmountCostForSaleToday { get; set; }


        public double AmountSalePercentInAllProductsToday { get; set; }


        public double AmountReceivedToday { get; set; }


        public double AmountReceivableToday { get; set; } // Due


        public double AmountAverageSalePriceToday { get; set; }


        public double AmountProfitToday { get; set; }


        public double AmountProfitPercentToday { get; set; }


        public double AmountProfitPercentInAllProductsToday { get; set; }


        public double AmountSaleToDealerToday { get; set; }


        public double AmountSaleToCustomerToday { get; set; }

        #endregion

        #region Amounts Purchase

        public double AmountPurchaseToday { get; set; }


        public double AmountPurchasePercentInAllProductsToday { get; set; }


        public double AmountPaidToday { get; set; }


        public double AmountPayableToday { get; set; }


        public double AmountAveragePurchasePricePerUnitToday { get; set; }

        #endregion
    }
}