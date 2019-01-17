using System;
using System.Data.Entity;
using System.Linq;
using CommonLibrary.Service;

using Vm = ViewModel.Customers.CustomerViewModel;
using Rm = RequestModel.Customers.CustomerRequestModel;
using M = Model.Customers.Customer;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Customers.Customer>;
using Model.Sales;
using ViewModel.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;

using Model;

namespace ServiceLibrary.Customers
{
    using ViewModel.Customers;

    public class CustomerService : BaseService<M, Rm, Vm>
    {
        private BusinessDbContext db;

        public CustomerService(Repo repository) : base(repository)
        {
            db = Repository.Db as BusinessDbContext;
        }

        public bool UpdatePoint(string customerId)
        {
            M customer = Repository.Get().Include(x => x.BuyingHistory).FirstOrDefault(x => x.Id == customerId);

            bool updatePoint = false;
            if (customer?.BuyingHistory != null)
            {
                var sales = customer.BuyingHistory;
                double productAmount = sales.Sum(x => x.ProductAmount);
                double discount = sales.Sum(x => x.DiscountAmount);
                double orderAmount = sales.Sum(x => x.PayableTotalAmount);

                var transactions = this.db.Transactions.Where(x => x.ParentId == customerId);
                var paids = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Income);

                double customerPaidTotal = 0;
                if (paids.Any())
                {
                    customerPaidTotal = paids.Sum(x => x.Amount);
                }

                var returneds = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Expense);
                double customerReturnedTotal = 0;
                if (returneds.Any())
                {
                    customerReturnedTotal = returneds.Sum(x => x.Amount);
                }

                double actualPaid = customerPaidTotal - customerReturnedTotal;

                customer.Point = (int)(actualPaid / 100);
                customer.OrdersCount = sales.Count();

                customer.ProductAmount = productAmount;
                customer.TotalDiscount = discount;
                customer.TotalAmount = orderAmount;
                customer.TotalPaid = actualPaid;
                customer.TotalDue = orderAmount - actualPaid;

                updatePoint = this.Edit(customer);
            }

            return updatePoint;
        }

        public string GetBarcode(string shopId)
        {
            int count = Repository.Get().Count(x => x.ShopId == shopId) + 1;
            String s = DateTime.Now.Year + string.Empty + DateTime.Now.Month.ToString().PadLeft(2, '0') + string.Empty +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
            var queryable = Repository.Get();
            do
            {
                var barcode = s + count.ToString().PadLeft(10, '0');
                var prod = queryable.FirstOrDefault(x => x.MembershipCardNo == barcode) == null;
                if (prod)
                {
                    return barcode;
                }
                count = count + 1;
            } while (true);
        }

        public async Task<Tuple<List<CustomerProductViewModel>, int>> GetCustomerProductView(Rm rm)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            IQueryable<SaleDetail> saleDetails = db.SaleDetails.Include(x => x.Sale).Where(x=> x.Sale.CustomerId == rm.ParentId).Include(x => x.ProductDetail).Where(x => x.ShopId == rm.ShopId);

            List<SaleDetail> models = await saleDetails.ToListAsync();
            List<SaleDetailViewModel> viewModels = models.Select(x => new SaleDetailViewModel(x)).ToList();
            List<CustomerProductViewModel> historyViewModels = viewModels.ConvertAll(x => new CustomerProductViewModel(x, x.ProductDetailName, x.SalePricePerUnit)).ToList();

            return new Tuple<List<CustomerProductViewModel>, int>(historyViewModels, viewModels.Count);
        }
        
        //public async Task<Tuple<List<HistoryViewModel>, int>> GetHistory(Rm rm)
        //{
        //    SaleService saleService = new SaleService(new BaseRepository<Sale>(Repository.Db));
        //    SaleRequestModel saleRequestModel =
        //        new SaleRequestModel(string.Empty) { ShopId = rm.ShopId, CustomerId = rm.ParentId, Page = -1 };
        //    Tuple<List<SaleViewModel>, int> salesTuple = await saleService.SearchAsync(saleRequestModel);
        //    List<HistoryViewModel> saleHistories = salesTuple.Item1.ConvertAll(x => new HistoryViewModel(x)).ToList();

        //    var transactionService = new BaseService<Transaction, TransactionRequestModel, TransactionViewModel>(new BaseRepository<Transaction>(Repository.Db));
        //    var transactionRequestModel =
        //        new TransactionRequestModel(string.Empty, "Modified", "False")
        //        { ShopId = rm.ShopId, ParentId = rm.ParentId, Page = -1 };
        //    Tuple<List<TransactionViewModel>, int> transactionsTuple =
        //        await transactionService.SearchAsync(transactionRequestModel);


        //    List<HistoryViewModel> trxHistories =
        //        transactionsTuple.Item1.ConvertAll(x => new HistoryViewModel(x) { Type = "Payment", SaleId = x.OrderId }).ToList();
        //    saleHistories.AddRange(trxHistories);

        //    List<HistoryViewModel> merged = saleHistories.OrderBy(x => x.Date).ToList();
        //    return new Tuple<List<HistoryViewModel>, int>(merged, merged.Count);
        //}


    }
}

