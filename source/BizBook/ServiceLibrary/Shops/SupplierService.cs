using Vm = ViewModel.Shops.SupplierViewModel;
using Rm = RequestModel.Shops.SupplierRequestModel;
using M = Model.Shops.Supplier;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Shops.Supplier>;

namespace ServiceLibrary.Shops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model.Purchases;
    using Model.Transactions;

    using RequestModel.Purchases;
    using RequestModel.Transactions;

    using Purchases;

    using ViewModel.History;
    using ViewModel.Purchases;
    using ViewModel.Transactions;
    using System.Data.Entity;
    using Model;

    public class SupplierService : BaseService<M, Rm, Vm>
    {
        private BusinessDbContext db;
        public SupplierService(Repo repository) : base(repository)
        {
            db = Repository.Db as BusinessDbContext;
        }

        public async Task<Tuple<List<HistoryViewModel>, int>> GetHistory(Rm rm)
        {
            PurchaseService service = new PurchaseService(new BaseRepository<Purchase>(Repository.Db));
            PurchaseRequestModel request = new PurchaseRequestModel("")
            { ShopId = rm.ShopId, ParentId = rm.ParentId, Page = -1 };
            Tuple<List<PurchaseViewModel>, int> result = await service.SearchAsync(request);
            List<HistoryViewModel> viewModels = result.Item1.ConvertAll(x => new HistoryViewModel(x)).ToList();
            var transactionService = new BaseService<Transaction, TransactionRequestModel, TransactionViewModel>(new BaseRepository<Transaction>(Repository.Db));
            Tuple<List<TransactionViewModel>, int> transactionTuple =
                await transactionService.SearchAsync(
                    new TransactionRequestModel("", "Modified", "False")
                    {
                        ShopId = rm.ShopId,
                        ParentId = rm.ParentId,
                        Page = -1
                    });

            List<HistoryViewModel> models =
                transactionTuple.Item1.ConvertAll(x => new HistoryViewModel(x) { Type = "Payment", PurchaseId = x.ParentId }).ToList();
            viewModels.AddRange(models);
            List<HistoryViewModel> merged = viewModels.OrderByDescending(x => x.Date).ToList();
            return new Tuple<List<HistoryViewModel>, int>(merged, merged.Count);
        }

        public bool UpdateAmount(string id)
        {
            var supplier = Repository.Get().Include(x => x.Purchases).FirstOrDefault(x => x.Id == id);

            bool updatePoint = false;
            if (supplier?.Purchases != null)
            {
                var purchases = supplier.Purchases.Where(x => x.IsActive);
                double productAmount = purchases.Sum(x => x.ProductAmount);
                double discount = purchases.Sum(x => x.DiscountAmount);
                double totalAmount = purchases.Sum(x => x.TotalAmount);

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

                supplier.OrdersCount = purchases.Count();
                supplier.ProductAmount = productAmount;
                supplier.TotalDiscount = discount;
                supplier.TotalAmount = totalAmount;
                supplier.TotalPaid = actualPaid;
                supplier.TotalDue = totalAmount - actualPaid;

                updatePoint = this.Edit(supplier);
            }

            return updatePoint;
        }
    }
}
