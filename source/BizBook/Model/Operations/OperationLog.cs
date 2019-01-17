using System.Collections.Generic;

namespace Model.Operations
{
   public class OperationLog : ShopChild
    {
        public OperationType OperationType { get; set; }
        public ModelName ModelName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectIdentifier { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<OperationLogDetail> OperationLogDetails { get; set; }
    }
}
