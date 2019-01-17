using System.Linq;

namespace ServiceLibrary.Dealers
{
    using System.Data.Entity;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Dealers;
    using RequestModel.Shops;
    using ViewModel.Shops;

    public class DealerService : BaseService<Dealer, DealerRequestModel, DealerViewModel>
    {
        private BusinessDbContext db;

        public DealerService(BaseRepository<Dealer> repository)
            : base(repository)
        {
            db = Repository.Db as BusinessDbContext;
        }

        public bool UpdateAmount(string id)
        {
            var dealer = Repository.Get().Include(x => x.Sales).FirstOrDefault(x => x.Id == id);

            bool updatePoint = false;
            if (dealer?.Sales != null)
            {
                var sales = dealer.Sales.Where(x => x.IsActive);
                double productAmount = sales.Sum(x => x.ProductAmount);
                double discount = sales.Sum(x => x.DiscountAmount);
                double orderAmount = sales.Sum(x => x.PayableTotalAmount);

                var transactions = this.db.Transactions.Where(x => x.ParentId == id);
                var paids = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Income);

                double paidTotal = 0;
                if (paids.Any())
                {
                    paidTotal = paids.Sum(x => x.Amount);
                }

                var returneds = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Expense);
                double returnedTotal = 0;
                if (returneds.Any())
                {
                    returnedTotal = returneds.Sum(x => x.Amount);
                }

                double actualPaid = paidTotal - returnedTotal;
                 
                dealer.OrdersCount = sales.Count();
                dealer.ProductAmount = productAmount;
                dealer.TotalDiscount = discount;
                dealer.TotalAmount = orderAmount;
                dealer.TotalPaid = actualPaid;
                dealer.TotalDue = orderAmount - actualPaid;

                updatePoint = this.Edit(dealer);
            }

            return updatePoint;
        }
    }
}
