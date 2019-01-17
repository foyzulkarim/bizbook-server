using System.Linq;
using CommonLibrary.Service;
using Model;
using Model.Purchases;
using M = Model.Transactions.Transaction;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Transactions.Transaction>;
using Rm = RequestModel.Transactions.TransactionRequestModel;
using Vm = ViewModel.Transactions.TransactionViewModel;

namespace ServiceLibrary.Transactions
{
    using System.Transactions;

    using CommonLibrary.Repository;

    using Model.Customers;
    using Model.Sales;

    using ServiceLibrary.Customers;

    public class TransactionService : BaseService<M, Rm, Vm>
    {
        //private AccountReportService2 accountReportService;

        private BusinessDbContext db;
        public TransactionService(Repo repository) : base(repository)
        {
          //  this.accountReportService = new AccountReportService2();
            db = Repository.Db as BusinessDbContext;
        }

        public override bool Add(M entity)
        {
            using (var scope = new TransactionScope())
            {
                bool saved = base.Add(entity);

                if (saved)
                {
                    UpdateRelatedTables(entity);
                }

                scope.Complete();
            }

            //this.accountReportService.QuickUpdate(entity.ShopId, entity.AccountHeadId, entity.Created);
            return true;
        }

        public override bool Edit(M entity)
        {
            using (var scope = new TransactionScope())
            {
                bool saved = base.Edit(entity);

                if (saved)
                {
                    UpdateRelatedTables(entity);
                }

                scope.Complete();
            }

           // this.accountReportService.QuickUpdate(entity.ShopId, entity.AccountHeadId, entity.Created);
            return true;
        }

        private void UpdateRelatedTables(M entity)
        {
            if (entity.TransactionFor == TransactionFor.Sale)
            {
                UpdateSaleRelatedTables(entity);
            }

            if (entity.TransactionFor == TransactionFor.Purchase)
            {
                UpdatePurchaseRelatedTables(entity);
            }

            this.Repository.Save();
        }

        private void UpdatePurchaseRelatedTables(M entity)
        {
            Purchase purchase = db.Purchases.FirstOrDefault(p => p.Id == entity.OrderId);
            if (purchase != null)
            {
                purchase.PaidAmount = purchase.PaidAmount + entity.Amount;
                purchase.DueAmount = purchase.TotalAmount - purchase.PaidAmount;
            }
        }

        private void UpdateSaleRelatedTables(M entity)
        {
            var sale = db.Sales.FirstOrDefault(item => item.Id == entity.OrderId);
            if (sale != null)
            {
                IQueryable<M> transactions = this.Repository.Get()
                    .Where(x => x.OrderId == entity.OrderId);
                double paidTotal = 0;
                var incomes = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Income)
                    .Select(x => x.Amount);
                if (incomes.Any())
                {
                    paidTotal = incomes.Sum();
                }

                double returnedTotal = 0;
                var expenses = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Expense)
                    .Select(x => x.Amount);
                if (expenses.Any())
                {
                    returnedTotal = expenses.Sum();
                }

                sale.PaidAmount = paidTotal - returnedTotal;

                sale.DueAmount = sale.PayableTotalAmount - sale.PaidAmount;

                if (sale.DueAmount == 0)
                {
                    IQueryable<SaleDetail> saleDetails = this.db.SaleDetails.Where(x => x.SaleId == sale.Id);
                    foreach (var detail in saleDetails)
                    {
                        detail.PaidAmount = detail.Total;
                        detail.DueAmount = 0;
                    }
                }

                this.db.SaveChanges();
            }

            if (!sale.IsDealerSale)
            {
                CustomerService customerService = new CustomerService(new BaseRepository<Customer>(this.db));
                customerService.UpdatePoint(sale.CustomerId);
            }
        }
    }
}
