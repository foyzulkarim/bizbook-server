using System;
using System.Collections.Generic;

namespace Model.Warehouses
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class StockTransfer : ShopChild
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
        public double ProductAmount { get; set; }

        [MaxLength(50)] public string DeliveryTrackingNo { get; set; }

        [MaxLength(50)] public string DeliverymanId { get; set; }

        [MaxLength(50)] public string DeliverymanName { get; set; }

        [MaxLength(50)] public string DeliverymanPhone { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EstimatedDeliveryDate { get; set; }

        public string SourceWarehouseId { get; set; }

        [ForeignKey("SourceWarehouseId")] public virtual Warehouse SourceWarehouse { get; set; }

        public string DestinationWarehouseId { get; set; }

        [ForeignKey("DestinationWarehouseId")] public virtual Warehouse DestinationWarehouse { get; set; }

        [MaxLength(200)] public string Remarks { get; set; }

        public StockTransferState TransferState { get; set; }

        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }
    }
}