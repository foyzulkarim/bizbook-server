namespace RequestModel.Reports
{
    using System;

    public class CustomerBySaleRequestModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ShopId { get; set; }

        public double MinimumAmountSpend { get; set; }

        public string Keyword { get; set; }

        public string WarehouseId { get; set; }
    }
}