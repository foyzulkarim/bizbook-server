using System;

namespace ViewModel.Warehouses
{
    public class WarehouseHistoryViewModel
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }

        public string Type { get; set; }

        public string ProductName { get; set; }

        public double Quantity { get; set; }

        public DateTime Date { get; set; }
    }
}