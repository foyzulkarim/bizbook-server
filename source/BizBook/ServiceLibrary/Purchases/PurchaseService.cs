using System;
using System.Data.Entity;
using System.Linq;

using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Products;
using Model.Purchases;
using ViewModel.Purchases;
using M = Model.Purchases.Purchase;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Purchases.Purchase>;
using Rm = RequestModel.Purchases.PurchaseRequestModel;
using Vm = ViewModel.Purchases.PurchaseViewModel;

namespace ServiceLibrary.Purchases
{
    using System.Collections.Generic;
    using System.Transactions;
    using Model.Transactions;
    using Transaction = Model.Transactions.Transaction;
    using ViewModel.Transactions;
    using System.Threading.Tasks;

    using ServiceLibrary.Products;

    using ViewModel.History;

    public class PurchaseService : BaseService<M, Rm, Vm>
    {
        //ProductReportService2 reportService;

       // private AccountReportService2 accountReportService;

        public PurchaseService(Repo repository) : base(repository)
        {
            //this.reportService = new ProductReportService2();
            //this.accountReportService = new AccountReportService2();
        }

        public override Vm GetDetail(string id)
        {
            var dbContext = Repository.Db as BusinessDbContext;
            Purchase purchase = GetById(id);
            Vm purchaseViewModel = new Vm(purchase);
            purchaseViewModel.PurchaseDetails = dbContext.PurchaseDetails.Where(x => x.PurchaseId == id)
                .Include(x => x.ProductDetail)
                .ToList()
                .ConvertAll(x => new PurchaseDetailViewModel(x))
                .ToList();


            purchaseViewModel.Transactions = dbContext.Transactions.Where(x => x.OrderNumber == purchase.OrderNumber)
                .ToList().ConvertAll(y => new TransactionViewModel(y)).ToList();

            return purchaseViewModel;
        }

        public override bool Add(M purchase)
        {
            foreach (var detail in purchase.PurchaseDetails)
            {
                detail.Id = Guid.NewGuid().ToString();
                detail.PurchaseId = purchase.Id;
                detail.Created = purchase.Created;
                detail.CreatedFrom = purchase.CreatedFrom;
                detail.CreatedBy = purchase.CreatedBy;
                detail.Modified = purchase.Modified;
                detail.ModifiedBy = purchase.ModifiedBy;
                detail.ShopId = purchase.ShopId;
                detail.WarehouseId = purchase.WarehouseId;
            }

            purchase.State = PurchaseStates.Received.ToString();
            using (TransactionScope scope = new TransactionScope())
            {
                bool added = base.Add(purchase);
                if (added)
                {
                    UpdateProductDetail(purchase);
                    AddTransaction(purchase);

                    SupplierProductService supplierProductService=new SupplierProductService(new BaseRepository<SupplierProduct>(BusinessDbContext.Create()));
                    supplierProductService.UpsertProductsBySupplier(purchase);
                }
                scope.Complete();
            }

            UpdateProductReport(purchase);
            UpdateAccountReport(purchase);
            return true;
        }

        private void UpdateAccountReport(Purchase purchase)
        {
            AccountHead accountHead = this.GetPurchaseAccountHead(purchase);
            //this.accountReportService.QuickUpdate(purchase.ShopId, accountHead.Id, purchase.Created);
        }

        private void UpdateProductReport(Purchase purchase)
        {
            foreach (var detail in purchase.PurchaseDetails)
            {
               // reportService.QuickUpdate(purchase.ShopId, detail.ProductDetailId, purchase.Created);
            }
        }

        public async Task<Tuple<List<HistoryViewModel>, int>> GetProductHistoryAsync(Rm rm)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            IQueryable<PurchaseDetail> purchaseDetails = db.PurchaseDetails.Include(x => x.Purchase).Include(x => x.ProductDetail).Where(x => x.ShopId == rm.ShopId);

            purchaseDetails = purchaseDetails.Where(x => x.Purchase.SupplierId == rm.ParentId);


            List<PurchaseDetail> models = await purchaseDetails.ToListAsync();
            List<PurchaseDetailViewModel> viewModels = models.ConvertAll(x => new PurchaseDetailViewModel(x)).ToList();
            List<HistoryViewModel> historyViewModels = viewModels.ConvertAll(x => new HistoryViewModel(x, x.ProductDetailName, x.CostPricePerUnit)).ToList();
            var list = historyViewModels.GroupBy(x => x.ProductDetailId).ToList();

            var histories = new List<HistoryViewModel>();
            foreach (var v in list)
            {
                HistoryViewModel m = new HistoryViewModel();
                string pid = v.Key;
                var pList = v.ToList();
                m.ProductName = pList.First().ProductName;
                m.ProductDetailId = pid;
                m.Quantity = pList.Sum(x => x.Quantity);
                m.Total = pList.Sum(x => x.Total);
                if (m.Quantity > 0)
                {
                    m.UnitPrice = m.Total / m.Quantity;
                }

                m.Paid = pList.Sum(x => x.Paid);
                m.Due = pList.Sum(x => x.Due);
                histories.Add(m);
            }

            return new Tuple<List<HistoryViewModel>, int>(histories, histories.Count);
        }

        private AccountHead GetPurchaseAccountHead(Purchase purchase)
        {
            var db = this.Repository.Db as BusinessDbContext;
            AccountHead accountHead = db.AccountHeads.FirstOrDefault(x => x.Name == "Purchase" && x.ShopId == purchase.ShopId);
            if (accountHead == null)
            {
                accountHead = new AccountHead() { ShopId = purchase.ShopId, Name = "Purchase" };
                this.AddCommonValues(purchase, accountHead);
                db.AccountHeads.Add(accountHead);
                db.SaveChanges();
            }

            return accountHead;
        }

        public bool PurchaseReturn(Purchase purchase)
        {
            using (var scope = new TransactionScope())
            {
                var dbPurchase = Repository.GetById(purchase.Id);
                if (dbPurchase == null)
                {
                    var exception = new KeyNotFoundException("No item found by this id");
                    throw exception;
                }

                var detailRepository = new BaseRepository<PurchaseDetail>(Repository.Db);

                foreach (var detail in purchase.PurchaseDetails)
                {
                    var dbDetail = dbPurchase.PurchaseDetails.FirstOrDefault(x => x.Id == detail.Id);
                    if (dbDetail != null)
                    {
                        dbDetail.CostPricePerUnit = detail.CostPricePerUnit;
                        dbDetail.Quantity = detail.Quantity;
                        dbDetail.CostTotal = detail.CostTotal;
                        this.Repository.Db.SaveChanges();
                    }
                    else
                    {
                        AddCommonValues(purchase, detail);
                        detail.ShopId = purchase.ShopId;
                        detail.PurchaseId = purchase.Id;
                        detailRepository.Add(detail);
                    }
                }

                Repository.Db.SaveChanges();

                string purchaseId = dbPurchase.Id;
                UpdateProductDetail(purchase);
                this.UpdatePurchaseAmounts(purchaseId);
                scope.Complete();
            }

            UpdateProductReport(purchase);
            return true;
        }

        private void UpdatePurchaseAmounts(string purchaseId)
        {
            var db = this.Repository.Db as BusinessDbContext;
            var purchase = db.Purchases.First(x => x.Id == purchaseId);
            var purchaseDetails = db.PurchaseDetails.Where(x => x.PurchaseId == purchaseId);
            purchase.ProductAmount = purchaseDetails.Sum(x => x.CostTotal);
            purchase.TotalAmount = purchase.ProductAmount - purchase.DiscountAmount;
            var transactions = db.Transactions.Where(x => x.OrderId == purchaseId);
            var income = transactions
                .Where(x => x.TransactionFlowType == TransactionFlowType.Income).Select(x => x.Amount).ToList();
            double receivedTotal = income.Sum(x => x);
            var expense = transactions.Where(x => x.TransactionFlowType == TransactionFlowType.Expense).Select(x => x.Amount).ToList();
            double paidTotal = expense.Sum(x => x);
            purchase.PaidAmount = paidTotal - receivedTotal;
            purchase.DueAmount = purchase.TotalAmount - purchase.PaidAmount;
            db.SaveChanges();
        }

        private void AddTransaction(Purchase purchase)
        {
            var db = Repository.Db as BusinessDbContext;
            AccountHead accountHead = db.AccountHeads.FirstOrDefault(x => x.Name == "Purchase" && x.ShopId == purchase.ShopId);
            if (accountHead == null)
            {
                accountHead = new AccountHead()
                {
                    ShopId = purchase.ShopId,
                    Name = "Purchase"
                };
                AddCommonValues(purchase, accountHead);
                db.AccountHeads.Add(accountHead);
                db.SaveChanges();
            }

            Transaction transaction = new Transaction()
            {
                ShopId = purchase.ShopId,
                AccountHeadId = accountHead.Id,
                AccountHeadName = accountHead.Name,
                TransactionFlowType = TransactionFlowType.Expense,
                Amount = purchase.PaidAmount,
                OrderNumber = purchase.OrderNumber,
                OrderId = purchase.Id,
                ParentId = purchase.SupplierId,
                Remarks = "initial payment",
                TransactionFor = TransactionFor.Purchase,
                TransactionWith = TransactionWith.Supplier,
                TransactionMedium = TransactionMedium.Cash,
                TransactionMediumName = "Cash",
                PaymentGatewayService = PaymentGatewayService.Cash,
                PaymentGatewayServiceName = "Cash",
                TransactionDate = DateTime.Now
            };

            AddCommonValues(purchase, transaction);
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public override bool Upsert(Purchase purchase)
        {
            var detailRepository = new BaseRepository<PurchaseDetail>(Repository.Db);
            bool success;
            purchase.Modified = DateTime.Now;

            if (!Repository.Exists(purchase.Id))
            {
                success = Add(purchase);
            }
            else
            {
                var temp = purchase.PurchaseDetails;
                purchase.PurchaseDetails = null;

                var existingPurchaseDetailsInDb = detailRepository.Get().Where(x => x.PurchaseId == purchase.Id).Select(x => x.Id).ToList();
                foreach (string s in existingPurchaseDetailsInDb)
                {
                    detailRepository.Delete(s);
                    detailRepository.Save();
                }

                success = Edit(purchase);
                foreach (var detail in temp)
                {
                    if (detailRepository.Exists(detail.Id))
                    {
                        detailRepository.Edit(detail);
                    }
                    else
                    {
                        detailRepository.Add(detail);
                    }
                    detailRepository.Save();
                }
            }

            if (success)
            {
                UpdateProductDetail(purchase);
            }

            return success;
        }

        private void UpdateProductDetail(Purchase purchase)
        {
            if (purchase.PurchaseDetails != null)
            {
                var collection = purchase.PurchaseDetails.Select(x => x.ProductDetailId).ToList();
                foreach (var id in collection)
                {
                    double purchased = Repository.Db.Set<PurchaseDetail>()
                        .Where(x => x.ProductDetailId == id)
                        .Sum(y => y.Quantity);
                    ProductDetail productDetail = Repository.Db.Set<ProductDetail>().Find(id);
                    if (productDetail != null)
                    {
                        productDetail.Purchased = purchased;
                        productDetail.OnHand = productDetail.Purchased + productDetail.StartingInventory - productDetail.Sold;                         
                    }

                    var warehouseProduct = this.Repository.Db.Set<WarehouseProduct>().FirstOrDefault(
                        x => x.ShopId == purchase.ShopId && x.ProductDetailId == id && x.WarehouseId == purchase.WarehouseId);
                    if (warehouseProduct == null)
                    {
                        warehouseProduct = new WarehouseProduct
                                               {
                                                   ShopId = purchase.ShopId,
                                                   WarehouseId = purchase.WarehouseId,
                                                   MinimumStockToNotify =
                                                       productDetail.MinimumStockToNotify,
                                                   ProductDetailId = productDetail.Id,
                                               };
                        this.AddCommonValues(purchase, warehouseProduct);
                        this.Repository.Db.Set<WarehouseProduct>().Add(warehouseProduct);
                        Repository.Save();
                    }

                    var purchaseByWh = this.Repository.Db.Set<PurchaseDetail>()
                        .Where(x => x.ShopId == purchase.ShopId && x.ProductDetailId == id && x.WarehouseId == purchase.WarehouseId)
                        .Sum(y => y.Quantity);
                    warehouseProduct.Purchased = purchaseByWh;
                    warehouseProduct.OnHand = warehouseProduct.Purchased + warehouseProduct.StartingInventory
                                                                         + warehouseProduct.TransferredIn
                                              - warehouseProduct.Sold - warehouseProduct.TransferredOut;
                    warehouseProduct.Modified = DateTime.Now;

                    Repository.Save();
                }
            }
        }
    }
}
