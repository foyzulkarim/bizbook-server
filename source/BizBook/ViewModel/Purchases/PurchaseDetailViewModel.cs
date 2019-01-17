using CommonLibrary.ViewModel;
using Model.Purchases;

namespace ViewModel.Purchases
{
    public class PurchaseDetailViewModel : BaseViewModel<PurchaseDetail>
    {
        public PurchaseDetailViewModel(PurchaseDetail x) : base(x)
        {
            ProductDetailId = x.ProductDetailId;
            PurchaseId = x.PurchaseId;
            Quantity = x.Quantity;
            CostPricePerUnit = x.CostPricePerUnit;
            TotalAmount = x.CostTotal;
            Remarks = x.Remarks;
            IsActive = x.IsActive;
            CostTotal = x.CostTotal;
            if (x.ProductDetail != null)
            {
                ProductDetailName = x.ProductDetail.Name;
                ProductCategoryId = x.ProductDetail.ProductCategoryId;
            }

            if (x.Purchase != null)
            {
                PurchaseOrderNo = x.Purchase.OrderNumber;
            }

            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }
        public string ProductDetailId { get; set; }
        public string ProductDetailName { get; set; }
        public string PurchaseId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public double Quantity { get; set; }
        public double CostPricePerUnit { get; set; }
        public double TotalAmount { get; set; }
        public string Remarks { get; set; }

        public string ProductCategoryId { get; set; }
        public double CostPrice { get; set; }
        public double CostTotal { get; set; }
    }
}