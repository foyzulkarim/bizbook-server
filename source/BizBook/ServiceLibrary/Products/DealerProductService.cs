using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Products;
using Model.Sales;
using RequestModel.Products;
using ViewModel.Products;

namespace ServiceLibrary.Products
{
    using System.Transactions;

    using Model.Dealers;
    using Model.Transactions;

    using ServiceLibrary.Dealers;

    public class DealerProductService : BaseService<DealerProduct, DealerProductRequestModel, DealerProductViewModel>
    {
        public DealerProductService(BaseRepository<DealerProduct> repository) : base(repository)
        {
        }

        public bool UpsertProductsByDealer(Sale sale)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            List<string> productDetailIds = db.Sales.Where(x => x.DealerId == sale.DealerId && x.ShopId == sale.ShopId).Include(x => x.SaleDetails)
                .SelectMany(x => x.SaleDetails).Select(x => x.ProductDetailId).Distinct().ToList();

            //foreach (SaleDetail detail in sale.SaleDetails)
            foreach (var productDetailId in productDetailIds)
            {
                List<SaleDetail> saleDetailsByProduct =
                    db.Sales.Where(x => x.DealerId == sale.DealerId && x.ShopId == sale.ShopId)
                        .Include(x => x.SaleDetails).SelectMany(x => x.SaleDetails)
                        .Where(x => x.ProductDetailId == productDetailId).ToList();

                double productQuantity = saleDetailsByProduct.Sum(x => x.Quantity);
                var productTotalPrice = saleDetailsByProduct.Sum(x => x.Total);

                var product = db.DealerProducts.FirstOrDefault(x =>
                    x.ShopId == sale.ShopId && x.DealerId == sale.DealerId &&
                    x.ProductDetailId == productDetailId);

                if (product == null)
                {
                    product = new DealerProduct
                    {
                        ProductDetailId = productDetailId,
                        DealerId = sale.DealerId,
                        Quantity = productQuantity,
                        TotalPrice = productTotalPrice,
                        ShopId = sale.ShopId,
                    };
                    product.Due = product.TotalPrice - product.Paid;
                    AddCommonValues(sale, product);
                    // populate with other info
                    db.DealerProducts.Add(product);
                    db.SaveChanges();
                }
                else
                {
                    product.Quantity = productQuantity;
                    product.TotalPrice = productTotalPrice;
                    product.Due = product.TotalPrice - product.Paid;

                    db.SaveChanges();
                }
            }

            return true;
        }


        public bool UpdateDues(DealerProductDetailUpdateModel model)
        {
            using (var scope = new TransactionScope())
            {
                var db = Repository.Db as BusinessDbContext;
                AccountHead head = db.AccountHeads.First(x => x.ShopId == model.ShopId && x.Name == "Sale");
                model.Transaction.AccountHeadId = head.Id;
                model.Transaction.AccountHeadName = head.Name;
                model.Transaction.TransactionFlowType = TransactionFlowType.Income;
                model.Transaction.ParentId = model.DealerId;
                model.Transaction.TransactionFor = TransactionFor.Sale;
                model.Transaction.TransactionWith = TransactionWith.Dealer;
                db.DealerProductTransactions.AddRange(model.DealerProductTransactions);
                db.Transactions.Add(model.Transaction);
                int changes = db.SaveChanges();
                
                foreach (var transaction in model.DealerProductTransactions)
                {
                    var dealerProduct = db.DealerProducts.First(x => x.Id == transaction.DealerProductId);
                    dealerProduct.Paid = db.DealerProductTransactions.Where(x => x.DealerProductId == dealerProduct.Id)
                        .Sum(x => x.Amount);
                    dealerProduct.Due = dealerProduct.TotalPrice - dealerProduct.Paid;
                    db.SaveChanges();
                }
                
                DealerService dealerService = new DealerService(new BaseRepository<Dealer>(Repository.Db));
                bool updatePoint = dealerService.UpdateAmount(model.DealerId);

                scope.Complete();

                return changes > 0;
            }            
        }
    }
}
