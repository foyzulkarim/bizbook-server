namespace Model.Products
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Model.Warehouses;

    public class WarehouseProduct : ShopChild
    {
        [Required] public int StartingInventory { get; set; } = 0;

        [Required] public double Purchased { get; set; }

        [Required] public double Sold { get; set; }

        [Required] public double TransferredIn { get; set; }

        [Required] public double TransferredOut { get; set; }

        [Required] public double OnHand { get; set; }

        public int MinimumStockToNotify { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }

        public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public virtual ProductDetail ProductDetail { get; set; }
    }
}