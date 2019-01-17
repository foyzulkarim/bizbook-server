using CommonLibrary.ViewModel;
using Model.Products;
using System.Collections.Generic;

namespace ViewModel.Products
{
    using Model;
    using Model.Transactions;

    public class DealerProductDetailUpdateModel : ShopChild
    {
        public string DealerId { get; set; }

        public Transaction Transaction { get; set; }

        public List<DealerProductTransaction> DealerProductTransactions { get; set; }
    }

    public class DealerProductViewModel : BaseViewModel<DealerProduct>
    {
        public DealerProductViewModel(DealerProduct x) : base(x)
        {
            ShopId = x.ShopId;
            Quantity = x.Quantity;
            TotalPrice = x.TotalPrice;
            Paid = x.Paid;
            Due = x.Due;
            ProductDetailId = x.ProductDetailId;
            DealerId = x.DealerId;
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

        public string DealerId { get; set; }

        [IsViewable] public string ProductName { get; set; }
    }
}