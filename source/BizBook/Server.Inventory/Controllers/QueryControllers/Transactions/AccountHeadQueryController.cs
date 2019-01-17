using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Rm = RequestModel.Transactions.AccountHeadRequestModel;
using M = Model.Transactions.AccountHead;
using Vm = ViewModel.Transactions.AccountHeadViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Transactions
{
    [RoutePrefix("api/AccountHeadQuery")]
    public class AccountHeadQueryController: BaseQueryController<M, Rm, Vm>
    {
        public AccountHeadQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }

       
    }
}