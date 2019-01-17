using System.Collections.Generic;
using CommonLibrary.Model;

namespace Model.Customers
{
    public class District : Entity
    {
        public string Name { get; set; }

        public string Name_Bn { get; set; }

        public virtual ICollection<Zone> Zones { get; set; }
    }
}