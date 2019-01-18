using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Transactions;
using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.QueryControllers.Transactions
{
    [RoutePrefix("api/WalletQuery")]
    public class WalletQueryController : BaseQueryController<Wallet,WalletRequestModel,WalletViewModel>
    {
      
        public WalletQueryController() : base(new BaseService<Wallet, WalletRequestModel, WalletViewModel>(new BaseRepository<Wallet>(BusinessDbContext.Create())))
        {
        }
    }
}
