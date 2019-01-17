using Model;

namespace ViewModel.Warehouses
{
    using CommonLibrary.ViewModel;
    using Model.Warehouses;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StockTransferViewModel : BaseViewModel<StockTransfer>
    {
        public StockTransferViewModel(StockTransfer x)
            : base(x)
        {
            this.ShopId = x.ShopId;
            this.SourceWarehouseId = x.SourceWarehouseId;
            this.DestinationWarehouseId = x.DestinationWarehouseId;
            this.OrderNumber = x.OrderNumber;
            this.OrderReferenceNumber = x.OrderReferenceNumber;
            this.ProductAmount = x.ProductAmount;
            this.DeliveryTrackingNo = x.DeliveryTrackingNo;
            this.DeliverymanId = x.DeliverymanId;
            this.DeliverymanName = x.DeliverymanName;
            this.DeliverymanPhone = x.DeliverymanPhone;
            this.EstimatedDeliveryDate = x.EstimatedDeliveryDate;
            this.Remarks = x.Remarks;
            this.TransferState = x.TransferState;

            if (x.SourceWarehouse != null)
            {
                this.SourceWarehouse = new WarehouseViewModel(x.SourceWarehouse);
            }

            if (x.SourceWarehouse != null)
            {
                this.SourceWarehouse = new WarehouseViewModel(x.SourceWarehouse);
            }

            if (x.DestinationWarehouse != null)
            {
                this.DestinationWarehouse = new WarehouseViewModel(x.DestinationWarehouse);
            }

            if (x.StockTransferDetails != null)
            {
                this.StockTransferDetails = x.StockTransferDetails.ToList()
                    .ConvertAll(y => new StockTransferDetailViewModel(y)).ToList();
            }
        }

        public StockTransferState TransferState { get; set; }
        
        public string OrderNumber { get; set; }

        public string OrderReferenceNumber { get; set; }

        public double ProductAmount { get; set; }

        public string DeliveryTrackingNo { get; set; }

        public string DeliverymanId { get; set; }

        public string DeliverymanName { get; set; }

        public string DeliverymanPhone { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public string Remarks { get; set; }

        public WarehouseViewModel SourceWarehouse { get; set; }

        public WarehouseViewModel DestinationWarehouse { get; set; }
        public string DestinationWarehouseId { get; set; }

        public string SourceWarehouseId { get; set; }

        public string ShopId { get; set; }

        public List<StockTransferDetailViewModel> StockTransferDetails { get; set; }
    }
}