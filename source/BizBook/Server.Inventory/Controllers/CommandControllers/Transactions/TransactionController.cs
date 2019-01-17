using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Transactions;
using RequestModel.Transactions;
using ViewModel.Transactions;
using TransactionService = ServiceLibrary.Transactions.TransactionService;

namespace Server.Inventory.Controllers.CommandControllers.Transactions
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : BaseCommandController<Transaction, TransactionRequestModel, TransactionViewModel>
    {
        public TransactionController() : base(new TransactionService(new BaseRepository<Transaction>(BusinessDbContext.Create())))
        {

        }


    }
}