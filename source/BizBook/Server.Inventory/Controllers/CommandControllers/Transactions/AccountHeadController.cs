using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.CommandControllers.Transactions
{
    using Model.Transactions;


    [RoutePrefix("api/AccountHead")]
    public class AccountHeadController : BaseCommandController<AccountHead, AccountHeadRequestModel, AccountHeadViewModel>
    {
        public AccountHeadController() : base(new BaseService<AccountHead, AccountHeadRequestModel, AccountHeadViewModel>(
            new BaseRepository<AccountHead>(BusinessDbContext.Create())))
        {

        }
    }
}