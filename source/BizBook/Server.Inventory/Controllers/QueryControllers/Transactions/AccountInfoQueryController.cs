using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Transactions;
using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.QueryControllers.Transactions
{
    [RoutePrefix("api/AccountInfoQuery")]
    public class AccountInfoQueryController : BaseQueryController<AccountInfo,AccountInfoRequestModel,AccountInfoViewModel>
    {
      
        public AccountInfoQueryController() : base(new BaseService<AccountInfo, AccountInfoRequestModel, AccountInfoViewModel>(new BaseRepository<AccountInfo>(BusinessDbContext.Create())))
        {
        }
    }
}
