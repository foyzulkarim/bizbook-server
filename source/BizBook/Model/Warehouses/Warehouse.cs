using System.ComponentModel.DataAnnotations.Schema;
using Model.Customers;

namespace Model.Warehouses
{
    public class Warehouse : ShopChild
    {
        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string Area { get; set; }

        public string District { get; set; }

        public bool IsMain { get; set; }
    }

    public class WarehouseZone : ShopChild
    {
        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }

        public string ZoneId { get; set; }

        [ForeignKey("ZoneId")] public virtual Zone Zone { get; set; }
    }
}