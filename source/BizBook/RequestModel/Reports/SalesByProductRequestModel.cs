namespace RequestModel.Reports
{
    using System;

    public class SalesByProductRequestModel
    {
        public string ShopId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Keyword { get; set; }

        public string WarehouseId { get; set; }
    }
}