using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model
{
    public enum PurchaseStates
    {
        All,
        Created,
        Accepted,
        ReadyToShip,
        ConfirmShip,
        Shipped,
        Received,
        Canceled
    }

    //public enum SaleReportTypes
    //{
    //    Individual,
    //    Daily,
    //    Weekly,
    //    Monthly,
    //    Yearly
    //}

    public enum SaleCommissionStates
    {
        Ordered,
        Accepted,
        Processing,
        Sold,
        Done
    }

    public enum CommunicationTypes
    {
        PhoneCall,
        Sms,
        Email,
        Inperson
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionMedium
    {
        All = 0,
        Cash = 1,
        Card = 2,
        Cheque = 3,
        Mobile = 4,
        Other = 5,
        Bank = 6
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionFor
    {
        All = 0,
        Sale = 1,
        Purchase = 2,
        Office = 3,
        Other = 4
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionWith
    {
        All = 0,
        Customer = 1,
        Supplier = 2,
        Employee = 3,
        Dealer = 4,
        Partner = 5,
        Other = 6
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccountInfoType
    {
        Cash,
        Bank,
        Mobile,
        Other
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentGatewayService
    {
        Cash = 1,
        Bkash,
        Rocket,
        Mcash,
        Ucash,
        Mycash,
        Easycash,
        Condition,
        Visa,
        MasterCard,
        AmericanExpress,
        Online,
        Cheque,
        Flexi,
        Other
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionFlowType
    {
        Income = 1,
        Expense = 2,
    }

    public enum AccountHeadType
    {
        Any,
        Asset,
        Liability,
        Equity,
        Expense,
        Income
    }

    public enum SaleChannel
    {
        All,
        InHouse,
        CashOnDelivery,
        Courier,
        Condition,
        Other
    }

    public enum SaleFrom
    {
        All,
        BizBook365,
        Facebook,
        Website,
        PhoneCall,
        MobileApp,
        Referral,
        Other,
    }

    public enum OrderState
    {
        //All, //0
        Pending = 1,
        Created, //2
        ReadyToDeparture, //3
        OnTheWay, // 4
        Delivered, //5
        Completed, //6
        Cancel // 7
    }

    public enum ReportTimeType
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SaleDetailType
    {
        Sale = 1,
        Damage,
        Gift,
        Return
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationType
    {
        Created = 1,
        Modified,
    }

    public enum SaleReportType
    {
        SaleByAmount,
        SaleByChannel,
        SaleByOrderFrom
    }


    public enum ProductReportType
    {
        ProductDetailHistory = 1,
        ProductDetailStockReport = 2
    }


    public enum AccountReportType
    {
        TransactionHistory = 1,
        TransactionDeatil = 2
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SmsReceiverType
    {
        Unknown = 0,
        Customer,
        Dealer,
        User,
        Supplier
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SmsReasonType
    {
        Unknown = 0,
        Sale,
        Purchase,
        Transaction
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BizSmsHook
    {
        OrderPending = 1,
        OrderCreated,
        OrderReadyToDepurture
    }

    public enum StockTransferState
    {
        Pending = 0,
        Approved = 1
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModelName
    {
        Sale = 1,
        Purchase,
        Transaction,
        SaleDetail,
        PurchaseDetail,
        Damage
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FeedbackType
    {
        Positive = 1,
        Negative,
        Other
    }
}