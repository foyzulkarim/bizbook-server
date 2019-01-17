using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Products;
using Model.Warehouses;

namespace Model.Sales
{
    public class SaleDetail : ShopChild
    {
        [Index] public int? WcId { get; set; }

        public int? WcProductId { get; set; }

        public int? WcProductVariationId { get; set; }

        [Required] public double Quantity { get; set; }

        [Required] public double CostPricePerUnit { get; set; }

        [Required] public double CostTotal { get; set; }

        public double ProductPricePerUnit { get; set; }

        public double DiscountPerUnit { get; set; }

        [Required] public double SalePricePerUnit { get; set; }

        [Required] public double PriceTotal { get; set; }
        
        public double DiscountTotal { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double Total { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double PaidAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double DueAmount { get; set; }


        [MaxLength(50)] public string ProductSerialNumber { get; set; }

        public bool IsReturned { get; set; }

        [MaxLength(200)] public string ReturnReason { get; set; }


        [Required] public string SaleId { get; set; }

        [ForeignKey("SaleId")] public virtual Sale Sale { get; set; }

        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }

        public string Remarks { get; set; }

        public SaleDetailType SaleDetailType { get; set; }
    }
}