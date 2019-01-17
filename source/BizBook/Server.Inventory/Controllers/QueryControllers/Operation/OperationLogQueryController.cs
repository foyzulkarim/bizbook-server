using Rm = RequestModel.Operation.OperationLogRequestModel;
using M = Model.Operations.OperationLog;
using Vm = ViewModel.Operation.OperationLogViewModel;
using System.Web.Http;
using Model;
using CommonLibrary.Service;
using CommonLibrary.Repository;

namespace Server.Inventory.Controllers.QueryControllers.operation
{
    [RoutePrefix("api/OperationLogQuery")]
    public class OperationLogQueryController : BaseQueryController<M, Rm, Vm>
    {
        public OperationLogQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }
        
    }
}