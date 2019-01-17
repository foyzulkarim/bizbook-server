using CommonLibrary.ViewModel;
using Model;
using Model.Operations;
using System.Collections.Generic;
using System.Linq;

namespace ViewModel.Operation
{
   public class OperationLogViewModel : BaseViewModel<OperationLog>
    {
        public OperationLogViewModel(OperationLog x) : base(x)
        {
            OperationType = x.OperationType;
            ModelName = x.ModelName;
            ObjectId = x.ObjectId;
            ObjectIdentifier = x.ObjectIdentifier;
            Remarks = x.Remarks;

            if (x.OperationLogDetails != null)
            {
                this.OperationLogDetails = x.OperationLogDetails.ToList().ConvertAll(y => new OperationLogDetailViewModel(y)).ToList();
                
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

        public List<OperationLogDetailViewModel> OperationLogDetails { get; set; }
       // public virtual ICollection<OperationLogDetailViewModel> OperationLogDetails { get; set; }
    }
}
