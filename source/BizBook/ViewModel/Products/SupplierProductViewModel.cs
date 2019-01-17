using CommonLibrary.ViewModel;
using Model.Products;
using System.Collections.Generic;
using Model.Transactions;
using Model;

namespace ViewModel.Products
{
    public class SupplierProductDetailUpdateModel : ShopChild
    {
        public string SupplierId { get; set; }

        public Transaction Transaction { get; set; }

        public List<SupplierProductTransaction> SupplierProductTransactions { get; set; }
    }

    public class SupplierProductViewModel : BaseViewModel<SupplierProduct>
    {
        public SupplierProductViewModel(SupplierProduct x) : base(x)
        {
            ShopId = x.ShopId;
            Quantity = x.Quantity;
            TotalPrice = x.TotalPrice;
            Paid = x.Paid;
            Due = x.Due;
            ProductDetailId = x.ProductDetailId;
            SupplierId = x.SupplierId;
            if (x.ProductDetail != null)
            {
                ProductName = x.ProductDetail.Name;
            }
        }

        public string ShopId { get; set; }

        public double Quantity { get; set; }
        public double TotalPrice { get; set; }

        public double Paid { get; set; }

        public double Due { get; set; }

        public string ProductDetailId { get; set; }

        public string SupplierId { get; set; }

        [IsViewable] public string ProductName { get; set; }
    }
}