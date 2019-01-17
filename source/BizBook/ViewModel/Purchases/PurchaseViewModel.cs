using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary.ViewModel;
using Model.Purchases;
using ViewModel.Shops;
using ViewModel.Transactions;

namespace ViewModel.Purchases
{
    public class PurchaseViewModel : BaseViewModel<Purchase>
    {
        public PurchaseViewModel(Purchase x) : base(x)
        {
            OrderNumber = x.OrderNumber;
            OrderReferenceNumber = x.OrderReferenceNumber;
            ShippingAmount = x.ShippingAmount;
            ProductAmount = x.ProductAmount;
            OtherAmount = x.OtherAmount;
            DiscountAmount = x.DiscountAmount;
            TotalAmount = x.TotalAmount;
            PaidAmount = x.PaidAmount;
            DueAmount = x.DueAmount;
            State = x.State;
            ShippingProvider = x.ShippingProvider;
            ShipmentTrackingNo = x.ShipmentTrackingNo;

            Remarks = x.Remarks;
            SupplierId = x.SupplierId;
            LastModified = x.Modified.ToString("yyyy MMMM dd");
            if (x.Supplier != null)
            {
                Supplier = new SupplierViewModel(x.Supplier);
                SupplierName = Supplier.Name;
            }

            if (x.PurchaseDetails != null)
            {
                PurchaseDetails = x.PurchaseDetails.ToList().ConvertAll(y => new PurchaseDetailViewModel(y)).ToList();
            }

            if (x.OrderDate != null)
            {
                this.OrderDate = x.OrderDate.Value;
            }

            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }

        [IsViewable] public string LastModified { get; set; }

        [IsViewable] public string OrderNumber { get; set; }
        public string OrderReferenceNumber { get; set; }
        public double ShippingAmount { get; set; } = 0;

        public double ProductAmount { get; set; }

        public double OtherAmount { get; set; } = 0;

        public double DiscountAmount { get; set; }

        [IsViewable] public double TotalAmount { get; set; }

        public double PaidAmount { get; set; }
        [IsViewable] public double DueAmount { get; set; }

        public string State { get; set; }

        public string ShippingProvider { get; set; }

        public string ShipmentTrackingNo { get; set; }

        public string SupplierId { get; set; }

        [IsViewable] public string SupplierName { get; set; }

        public string Remarks { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual SupplierViewModel Supplier { get; set; }
        public virtual ICollection<PurchaseDetailViewModel> PurchaseDetails { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
    }
}