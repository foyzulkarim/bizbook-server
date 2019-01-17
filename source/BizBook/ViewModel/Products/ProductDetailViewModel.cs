using CommonLibrary.ViewModel;
using Model.Products;

namespace ViewModel.Products
{
    public class ProductDetailViewModel : BaseViewModel<ProductDetail>
    {
        public ProductDetailViewModel(ProductDetail x) : base(x)
        {
            SetProperties(x);
        }

        private void SetProperties(ProductDetail x)
        {
            Name = x.Name;
            Model = x.Model;
            Year = x.Year;
            BarCode = x.BarCode;
            ProductCode = x.ProductCode;
            HasUniqueSerial = x.HasUniqueSerial;
            HasWarrenty = x.HasWarrenty;
            SalePrice = x.SalePrice;
            CostPrice = x.CostPrice;
            Type = x.Type;
            Color = x.Color;
            MinimumStockToNotify = x.MinimumStockToNotify;
            StartingInventory = x.StartingInventory;
            Purchased = x.Purchased;
            Sold = x.Sold;
            OnHand = x.OnHand;
            ProductCategoryId = x.ProductCategoryId;
            BrandId = x.BrandId;
            IsRawProduct = x.IsRawProduct;

            if (x.ProductCategory != null)
            {
                ProductName = x.ProductCategory.Name;
            }

            if (x.Brand != null)
            {
                BrandName = x.Brand.Name;
            }

            ShopId = x.ShopId;
            DealerPrice = x.DealerPrice;

            WcId = x.WcId;
            WcCategoryId = x.WcCategoryId;
            WcType = x.WcType;
            WcVariationId = x.WcVariationId;
            WcVariationOption = x.WcVariationOption;
            WcVariationPermalink = x.WcVariationPermalink;
        }

        public int WcId { get; set; }

        public int WcCategoryId { get; set; }

        public string WcType { get; set; }

        public int WcVariationId { get; set; }

        public string WcVariationOption { get; set; }

        public string WcVariationPermalink { get; set; }

        public string ShopId { get; set; }

        [IsViewable] public string Name { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public bool HasUniqueSerial { get; set; } = false;

        public bool HasWarrenty { get; set; } = false;

        [IsViewable] public double SalePrice { get; set; }

        [IsViewable] public double DealerPrice { get; set; }

        public double CostPrice { get; set; }

        public string Type { get; set; }

        public string Color { get; set; }

        public int MinimumStockToNotify { get; set; }

        public int StartingInventory { get; set; } = 0;

        public double Purchased { get; set; }

        public double Sold { get; set; }

        [IsViewable] public double OnHand { get; set; }

        public double StockIn { get; set; }

        public double StockOut { get; set; }

        public string ProductCategoryId { get; set; }

        [IsViewable] public string ProductName { get; set; }

        public string BrandId { get; set; }

        [IsViewable] public string BrandName { get; set; }

        public bool IsRawProduct { get; set; }
    }

    public class ProductSaleViewModel
    {
        public string Name { get; set; }
        public string ProductName { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public int OnHand { get; set; }
        public string Id { get; set; }
        public string StockId { get; set; }
    }


    public class VarianceDeserializedModel
    {
        public string VarianceId { get; set; }

        public string VarianceDetailId { get; set; }

        public string Variance { get; set; }

        public string VarianceDetail { get; set; }
    }
}