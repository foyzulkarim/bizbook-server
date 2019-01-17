using Rm = RequestModel.Operation.OperationLogDetailRequestModel;
using M = Model.Operations.OperationLogDetail;
using Vm = ViewModel.Operation.OperationLogDetailViewModel;
using System.Web.Http;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.QueryControllers.operation
{
    [RoutePrefix("api/OperationLogDetailQuery")]
    public class OperationLogDetailQueryController : BaseQueryController<M, Rm, Vm>
    {
        public OperationLogDetailQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }
    }
}