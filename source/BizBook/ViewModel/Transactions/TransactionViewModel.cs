namespace ViewModel.Transactions
{
    using CommonLibrary.ViewModel;
    using Model;
    using Model.Transactions;
    using System;

    public class TransactionViewModel : BaseViewModel<Transaction>
    {
        public TransactionViewModel(Transaction x) : base(x)
        {
            TransactionFor = x.TransactionFor;
            TransactionType = x.TransactionFlowType;
            TransactionFlowType = x.TransactionFlowType.ToString();
            TransactionWith = x.TransactionWith;
            TransactionMedium = x.TransactionMedium;
            TransactionMediumName = x.TransactionMediumName;
            ParentId = x.ParentId;
            OrderNumber = x.OrderNumber;
            Amount = x.Amount;
            TransactionNumber = x.TransactionNumber;
            Remarks = x.Remarks;
            ContactPersonName = x.ContactPersonName;

            if (x.TransactionDate != null)
            {
                this.TransactionDate = x.TransactionDate.Value;
            }

            ContactPersonPhone = x.ContactPersonPhone;
            AccountHeadName = x.AccountHeadName;
            AccountHeadId = x.AccountHeadId;
            ShopId = x.ShopId;
            OrderId = x.OrderId;
            PaymentGatewayService = x.PaymentGatewayService.ToString();
            PaymentGatewayServiceName = PaymentGatewayService;
            AccountInfoId = x.AccountInfoId;
            if (x.AccountInfo != null)
            {
                AccountInfoTitle = x.AccountInfo.AccountTitle;
            }
        }

        public string TransactionFlowType { get; set; }

        public string PaymentGatewayServiceName { get; set; }

        public string PaymentGatewayService { get; set; }

        public string OrderId { get; set; }

        public string ShopId { get; set; }

        public TransactionFor TransactionFor { get; set; }

        public TransactionWith TransactionWith { get; set; }

        public TransactionMedium TransactionMedium { get; set; }

        public string ParentId { get; set; }

        public string OrderNumber { get; set; }

        public double Amount { get; set; }

        public string TransactionMediumName { get; set; }

        public string TransactionNumber { get; set; }

        public string Remarks { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonPhone { get; set; }

        public DateTime TransactionDate { get; set; }

        public string AccountHeadName { get; set; }

        public TransactionFlowType TransactionType { get; set; }

        public string AccountHeadId { get; set; }

        public string AccountInfoId { get; set; }

        public string AccountInfoTitle { get; set; }
    }
}