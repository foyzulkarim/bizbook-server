using System.Web.Http;
using Rm = RequestModel.Sales.InstallmentRequestModel;
using M = Model.Sales.Installment;
using Vm = ViewModel.Sales.InstallmentViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Sales
{
    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;

    [RoutePrefix("api/InstallmentQuery")]
    public class InstallmentQueryController : BaseQueryController<M, Rm, Vm>
    {
        public InstallmentQueryController()
            : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }
    }
}
