using CommonLibrary.Service;
using Model.Products;
using RequestModel.Products;
using System.Collections.Generic;
using System.Linq;
using ViewModel.Products;
using CommonLibrary.Repository;
using Model.Purchases;
using Model;
using System.Data.Entity;
using System.Transactions;
using Model.Transactions;
using Model.Shops;
using ServiceLibrary.Shops;

namespace ServiceLibrary.Products
{
    public class SupplierProductService : BaseService<SupplierProduct, SupplierProductRequestModel, SupplierProductViewModel>
    {
        public SupplierProductService(BaseRepository<SupplierProduct> repository) : base(repository)
        {
        }

        public bool UpsertProductsBySupplier(Purchase purchase)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;

            var productDetailIds = db.Purchases.Where(x => x.SupplierId == purchase.SupplierId && x.ShopId == purchase.ShopId)
                .Include(x => x.PurchaseDetails).SelectMany(x => x.PurchaseDetails).Select(x => x.ProductDetailId)
                .Distinct().ToList();

            foreach(string productDetailId in productDetailIds)
            {
                List<PurchaseDetail> purchaseDetailsByProduct =
                    db.Purchases.Where(x => x.SupplierId == purchase.SupplierId && x.ShopId == purchase.ShopId)
                        .Include(x => x.PurchaseDetails).SelectMany(x => x.PurchaseDetails)
                        .Where(x => x.ProductDetailId == productDetailId).ToList();

                double productQuantity = purchaseDetailsByProduct.Sum(x => x.Quantity);
                var productTotalPrice = purchaseDetailsByProduct.Sum(x => x.CostTotal);

                var product = db.SupplierProducts.FirstOrDefault(x =>
                    x.ShopId == purchase.ShopId && x.SupplierId == purchase.SupplierId &&
                    x.ProductDetailId == productDetailId);

                if (product == null)
                {
                    product = new SupplierProduct
                    {
                        ProductDetailId = productDetailId,
                        SupplierId = purchase.SupplierId,
                        Quantity = productQuantity,
                        TotalPrice = productTotalPrice,
                        ShopId = purchase.ShopId,
                    };
                    product.Due = product.TotalPrice - product.Paid;
                    AddCommonValues(purchase, product);
                    db.SupplierProducts.Add(product);
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

        public bool UpdateDues(SupplierProductDetailUpdateModel model)
        {
            using (var scope = new TransactionScope())
            {
                var db = Repository.Db as BusinessDbContext;
                AccountHead head = db.AccountHeads.First(x => x.ShopId == model.ShopId && x.Name == "Purchase");
                model.Transaction.AccountHeadId = head.Id;
                model.Transaction.AccountHeadName = head.Name;
                model.Transaction.TransactionFlowType = TransactionFlowType.Expense;
                model.Transaction.ParentId = model.SupplierId;
                model.Transaction.TransactionFor = TransactionFor.Purchase;
                model.Transaction.TransactionWith = TransactionWith.Supplier;
                db.SupplierProductTransactions.AddRange(model.SupplierProductTransactions);
                db.Transactions.Add(model.Transaction);
                int changes = db.SaveChanges();

                foreach (var transaction in model.SupplierProductTransactions)
                {
                    var supplierProduct = db.SupplierProducts.First(x => x.Id == transaction.SupplierProductId);
                    supplierProduct.Paid = db.SupplierProductTransactions.Where(x => x.SupplierProductId == supplierProduct.Id)
                        .Sum(x => x.Amount);
                    supplierProduct.Due = supplierProduct.TotalPrice - supplierProduct.Paid;
                    db.SaveChanges();
                }

                SupplierService supplierService = new SupplierService(new BaseRepository<Supplier>(Repository.Db));
                bool updatePoint = supplierService.UpdateAmount(model.SupplierId);

                scope.Complete();

                return changes > 0;
            }
        }
    }
}
