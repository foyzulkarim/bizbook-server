using Rm = RequestModel.Operation.OperationLogRequestModel;
using M = Model.Operations.OperationLog;
using Vm = ViewModel.Operation.OperationLogViewModel;
using System.Web.Http;
using Model;
using CommonLibrary.Service;
using CommonLibrary.Repository;

namespace Server.Inventory.Controllers.CommandControllers.Operation
{
    [RoutePrefix("api/OperationLog")]
    public class OperationLogController : BaseQueryController<M, Rm, Vm>
    {
        public OperationLogController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }
    }
}