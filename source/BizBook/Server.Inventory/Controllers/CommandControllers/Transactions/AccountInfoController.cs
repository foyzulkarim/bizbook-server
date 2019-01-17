using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Transactions;
using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.CommandControllers.Transactions
{
    [RoutePrefix("api/AccountInfo")]
    public class AccountInfoController : BaseCommandController<AccountInfo, AccountInfoRequestModel, AccountInfoViewModel>
    {
        public AccountInfoController() : base(new BaseService<AccountInfo, AccountInfoRequestModel, AccountInfoViewModel>(new BaseRepository<AccountInfo>(BusinessDbContext.Create())))
        {
        }
    }
}
