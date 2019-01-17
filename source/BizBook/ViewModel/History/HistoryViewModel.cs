using System;

namespace ViewModel.History
{
    using Model;
    using Sales;
    using Purchases;
    using Transactions;
    using ViewModel.Warehouses;

    /// <summary>
    /// The history view model.
    /// </summary>
    public class HistoryViewModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public TransactionFlowType TransactionType { get; set; }

        public double Paid { get; set; }

        public double Total { get; set; }

        public double Due { get; set; }

        public string InvoiceNumber { get; set; }
        public OrderState OrderState { get; private set; }
        public string State { get; set; }
        public string TransactionNumber { get; set; }

        //public string IdentificationNumber { get; set; } // order number, transaction number etc
        public double OnHand { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductDetailId { get; set; }
        public string PurchaseId { get; set; }
        public string TransactionId { get; set; }
        public string SaleId { get; set; }
        public string StockTransferId { get; set; }
        public string TransferState { get; set; }
        public string Remarks { get; set; }
        public double Balance { get; set; }


        public HistoryViewModel()
        {
        }

        public HistoryViewModel(PurchaseViewModel model)
        {
            Id = model.Id;
            Date = model.Modified;
            Type = "Purchase";
            Total = model.TotalAmount;
            InvoiceNumber = model.OrderNumber;
            Paid = model.PaidAmount;
            PurchaseId = model.Id;
        }

        public HistoryViewModel(TransactionViewModel model)
        {
            Id = model.Id;
            Date = model.Modified;
            TransactionType = model.TransactionType;
            Total = model.Amount;
            Type = "Payment";
            InvoiceNumber = model.OrderNumber;
            TransactionNumber = model.TransactionNumber;
        }

        public HistoryViewModel(SaleViewModel model)
        {
            Id = model.Id;
            Date = model.Modified;
            Type = "Sale";
            Total = model.PayableTotalAmount;
            InvoiceNumber = model.OrderNumber;
            Paid = model.PaidAmount;
            SaleId = model.Id;
        }

        public HistoryViewModel(SaleDetailViewModel model, string productName, double salePrice)
        {
            Id = model.Id;
            Date = model.Modified;
            Type = "Sale";
            Total = model.Total;
            Quantity = model.Quantity;
            Paid = model.PaidAmount;
            Due = model.DueAmount;
            UnitPrice = salePrice;
            ProductName = productName;
            ProductDetailId = model.ProductDetailId;
            SaleId = model.SaleId;
            InvoiceNumber = model.SaleOrderNo;
            OrderState = model.OrderState;
            State = model.OrderState.ToString();
        }

        public HistoryViewModel(PurchaseDetailViewModel model, string productName, double costPrice)
        {
            Id = model.Id;
            Date = model.Modified;
            Type = "Purchase";
            Total = model.TotalAmount;
            Quantity = model.Quantity;
            UnitPrice = costPrice;
            ProductDetailId = model.ProductDetailId;
            ProductName = productName;
            PurchaseId = model.PurchaseId;
            InvoiceNumber = model.PurchaseOrderNo;
        }

        public HistoryViewModel(StockTransferDetailViewModel model, string productName, string type)
        {
            Id = model.Id;
            Date = model.Modified;
            Type = type;
            Total = model.PriceTotal;
            Quantity = model.Quantity;
            ProductDetailId = model.ProductDetailId;
            ProductName = productName;
            InvoiceNumber = model.StockTransferOrderNumber;
            StockTransferId = model.StockTransferId;
            TransferState = model.TransferState;
            Remarks = type + " " + model.StockTransferViewModel.SourceWarehouse.Name + " to " + model.StockTransferViewModel.DestinationWarehouse.Name;
        }

    }
}