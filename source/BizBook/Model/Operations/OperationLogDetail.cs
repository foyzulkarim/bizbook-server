using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Operations
{
    public class OperationLogDetail : ShopChild
    {

        public OperationType OperationType { get; set; }
        public ModelName ModelName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectIdentifier { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remarks { get; set; }      
        public string OperationLogId { get; set; }
        [ForeignKey("OperationLogId")] public virtual OperationLog OperationLog { get; set; }
    }
}
