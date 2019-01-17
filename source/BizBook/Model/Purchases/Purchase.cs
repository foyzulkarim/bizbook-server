using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Shops;
using Model.Warehouses;

namespace Model.Purchases
{
    public class Purchase : ShopChild
    {
        [Index]
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string OrderNumber { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string OrderReferenceNumber { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double ShippingAmount { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double ProductAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double OtherAmount { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double DiscountAmount { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double TotalAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double PaidAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double DueAmount { get; set; }

        [MaxLength(50)] public string State { get; set; }

        [MaxLength(50)] public string ShippingProvider { get; set; }

        [MaxLength(50)] public string ShipmentTrackingNo { get; set; }

        [Required] public string SupplierId { get; set; }

        [ForeignKey("SupplierId")] public virtual Supplier Supplier { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }

        [MaxLength(500)] public string Remarks { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OrderDate { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}