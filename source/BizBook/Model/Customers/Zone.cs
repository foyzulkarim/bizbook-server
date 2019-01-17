using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Model;

namespace Model.Customers
{
    public class Zone : Entity
    {
        public string Name { get; set; }

        public string DistrictId { get; set; }

        [ForeignKey("DistrictId")] public virtual District District { get; set; }
    }
}