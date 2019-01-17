using CommonLibrary.ViewModel;
using Model;
using Model.Operations;

namespace ViewModel.Operation
{
   public class OperationLogDetailViewModel : BaseViewModel<OperationLogDetail>
    {
        public OperationLogDetailViewModel(OperationLogDetail x) : base(x)
        {
            OperationType = x.OperationType;
            ModelName = x.ModelName;
            ObjectId = x.ObjectId;
            ObjectIdentifier = x.ObjectIdentifier;
            PropertyName = x.PropertyName;
            OldValue = x.OldValue;
            NewValue = x.NewValue;
            Remarks = x.Remarks;
           
            OperationLogId = x.OperationLogId;
            if (x.OperationLog != null)
            {
                OperationLogNo = x.OperationLog.ObjectIdentifier;
                x.OperationLog.OperationLogDetails = null;
                OperationLogViewModel = new OperationLogViewModel(x.OperationLog);
            }
        }

        public OperationType OperationType { get; set; }
        public ModelName ModelName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectIdentifier { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remarks { get; set; }
        public string OperationLogId { get; set; }
        public string OperationLogNo { get; set; }
        public string SaleOrderNumber { get; set; }
        public OperationLogViewModel OperationLogViewModel { get; set; }

    }
}
