using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Warehouses;

namespace Model.Employees
{
    public class EmployeeInfo : ShopChild
    {
        [Index] [Required] [MaxLength(200)] public string Name { get; set; }

        [Index] [MaxLength(100)] public string Phone { get; set; }

        [Index] [MaxLength(50)] public string Email { get; set; }
        public string RoleId { get; set; }
        public double Salary { get; set; }
        public double SaleTargetAmount { get; set; }
        public double SaleAchivedAmount { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }
    }
}