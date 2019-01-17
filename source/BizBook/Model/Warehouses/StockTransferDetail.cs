namespace Model.Warehouses
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Model.Products;

    public class StockTransferDetail : ShopChild
    {
        [Required] public double Quantity { get; set; }

        [Required] public double SalePricePerUnit { get; set; }

        [Required] public double PriceTotal { get; set; }

        [Required] public string StockTransferId { get; set; }

        [ForeignKey("StockTransferId")] public virtual StockTransfer StockTransfer { get; set; }

        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }

        public string SourceWarehouseId { get; set; }

        [ForeignKey("SourceWarehouseId")] public virtual Warehouse SourceWarehouse { get; set; }

        public string DestinationWarehouseId { get; set; }

        [ForeignKey("DestinationWarehouseId")] public virtual Warehouse DestinationWarehouse { get; set; }

        [MaxLength(200)] public string Remarks { get; set; }
    }
}