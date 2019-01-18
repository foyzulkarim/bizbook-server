using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Transactions;
using RequestModel.Transactions;
using ViewModel.Transactions;

namespace Server.Inventory.Controllers.CommandControllers.Transactions
{
    [RoutePrefix("api/Wallet")]
    public class WalletController : BaseCommandController<Wallet, WalletRequestModel, WalletViewModel>
    {
        public WalletController() : base(new BaseService<Wallet, WalletRequestModel, WalletViewModel>(new BaseRepository<Wallet>(BusinessDbContext.Create())))
        {
        }
    }
}
