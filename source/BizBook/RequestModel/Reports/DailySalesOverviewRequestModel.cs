using System;
using Model;

namespace RequestModel.Reports
{
    public class DailySalesOverviewRequestModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ShopId { get; set; }

        public string WarehouseId { get; set; }

        public string Keyword { get; set; }

        public SaleFrom SaleFrom { get; set; }
    }
}