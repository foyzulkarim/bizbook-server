using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model;

namespace ServiceLibrary.Warehouses
{
    using System.Transactions;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model.Products;
    using Model.Warehouses;
    using RequestModel.Warehouses;
    using ViewModel.Warehouses;

    public class StockTransferService : BaseService<StockTransfer, StockTransferRequestModel, StockTransferViewModel>
    {

        public StockTransferService(BaseRepository<StockTransfer> repository)
            : base(repository)
        {

        }

        public override bool Add(StockTransfer transfer)
        {
            using (var scope = new TransactionScope())
            {
                foreach (var detail in transfer.StockTransferDetails)
                {
                    AddCommonValues(transfer, detail);
                    detail.ShopId = transfer.ShopId;
                    detail.SourceWarehouseId = transfer.SourceWarehouseId;
                    detail.DestinationWarehouseId = transfer.DestinationWarehouseId;
                    detail.Remarks = transfer.Remarks;
                }

                transfer.OrderNumber = GetOrderNumber(transfer.ShopId);

                transfer.TransferState = StockTransferState.Pending;

                bool added = base.Add(transfer);               
                scope.Complete();
                return added;
            }
        }

        public bool UpdateState(string stockTransferId)
        {
            var db = base.Repository.Db as BusinessDbContext;
            var stockTransfer = db.StockTransfers.Include(x => x.StockTransferDetails)
                .FirstOrDefault(x => x.Id == stockTransferId);
            if (stockTransfer==null)
            {
                return false;
            }

            stockTransfer.TransferState = StockTransferState.Approved;
            int saveChanges = db.SaveChanges();
            UpdateProductDetail(stockTransfer);

            return true;
        }

        public override bool Edit(StockTransfer sale)
        {
            using (var scope = new TransactionScope())
            {
                BusinessDbContext db = this.Repository.Db as BusinessDbContext;
                StockTransfer dbSale = db.StockTransfers.Where(x => x.Id == sale.Id).Include(x => x.StockTransferDetails).FirstOrDefault();
                if (dbSale == null)
                {
                    var exception = new KeyNotFoundException("No sale found by this id");
                    throw exception;
                }
                
                var detailRepository = new BaseRepository<StockTransferDetail>(this.Repository.Db);

                foreach (var detail in sale.StockTransferDetails)
                {
                    var dbDetail = dbSale.StockTransferDetails.FirstOrDefault(x => x.Id == detail.Id);
                    if (dbDetail != null)
                    {
                        dbDetail.SalePricePerUnit = detail.SalePricePerUnit;                      
                        dbDetail.Quantity = detail.Quantity;
                        dbDetail.PriceTotal = detail.PriceTotal;
                        dbDetail.Remarks = detail.Remarks;
                        this.Repository.Db.SaveChanges();
                    }
                    else
                    {
                        this.AddCommonValues(sale, detail);
                        detail.ShopId = sale.ShopId;
                        detail.StockTransferId = sale.Id;                        
                        detailRepository.Add(detail);
                    }
                }

                dbSale.ProductAmount = sale.ProductAmount;
              
                this.Repository.Db.SaveChanges();
                scope.Complete();
            }

            return true;
        }

        private void UpdateProductDetail(StockTransfer transfer)
        {
            if (transfer.StockTransferDetails != null)
            {
                List<string> ids = transfer.StockTransferDetails.Select(x => x.ProductDetailId).ToList();
                foreach (var id in ids)
                {
                    ProductDetail productDetail = Repository.Db.Set<ProductDetail>().Find(id);

                    var db = this.Repository.Db as BusinessDbContext;
                    var sourceWarehouseProduct = db.WarehouseProducts.FirstOrDefault(
                        x => x.ShopId == transfer.ShopId && x.ProductDetailId == id && x.WarehouseId == transfer.SourceWarehouseId);
                    if (sourceWarehouseProduct == null)
                    {
                        sourceWarehouseProduct = new WarehouseProduct
                        {
                            ShopId = transfer.ShopId,
                            WarehouseId = transfer.SourceWarehouseId,
                            MinimumStockToNotify = productDetail.MinimumStockToNotify,
                            ProductDetailId = productDetail.Id,
                        };
                        this.AddCommonValues(transfer, sourceWarehouseProduct);
                        db.WarehouseProducts.Add(sourceWarehouseProduct);
                        Repository.Save();
                    }

                    var decreasedByWh = db.StockTransferDetails.Include(x=>x.StockTransfer)
                        .Where(x => x.ShopId == transfer.ShopId && x.ProductDetailId == id && x.SourceWarehouseId == transfer.SourceWarehouseId && x.StockTransfer.TransferState == StockTransferState.Approved).Sum(y => (double?)y.Quantity) ?? 0;

                    sourceWarehouseProduct.TransferredOut = decreasedByWh;
                    sourceWarehouseProduct.OnHand = sourceWarehouseProduct.Purchased
                                                    + sourceWarehouseProduct.TransferredIn
                                                    + sourceWarehouseProduct.StartingInventory
                                                    - sourceWarehouseProduct.Sold
                                                    - sourceWarehouseProduct.TransferredOut;
                    sourceWarehouseProduct.Modified = DateTime.Now;

                    var destinationWarehouseProduct = db.WarehouseProducts.FirstOrDefault(
                        x => x.ShopId == transfer.ShopId && x.ProductDetailId == id && x.WarehouseId == transfer.DestinationWarehouseId);
                    if (destinationWarehouseProduct == null)
                    {
                        destinationWarehouseProduct = new WarehouseProduct
                        {
                            ShopId = transfer.ShopId,
                            WarehouseId = transfer.DestinationWarehouseId,
                            MinimumStockToNotify = productDetail.MinimumStockToNotify,
                            ProductDetailId = productDetail.Id,
                        };
                        this.AddCommonValues(transfer, destinationWarehouseProduct);
                        db.WarehouseProducts.Add(destinationWarehouseProduct);
                        Repository.Save();
                    }

                    double increasedByWh = db.StockTransferDetails.Include(x => x.StockTransfer)
                                               .Where(x => x.ShopId == transfer.ShopId 
                                                           && x.StockTransfer.TransferState == StockTransferState.Approved
                                                           && x.ProductDetailId == id
                                                           && x.DestinationWarehouseId
                                                           == transfer.DestinationWarehouseId)
                                               .Sum(x => (double?) x.Quantity) ?? 0;
                    destinationWarehouseProduct.TransferredIn = increasedByWh;
                    destinationWarehouseProduct.OnHand = destinationWarehouseProduct.Purchased
                                                         + destinationWarehouseProduct.StartingInventory
                                                         + destinationWarehouseProduct.TransferredIn
                                                         - destinationWarehouseProduct.Sold
                                                         - destinationWarehouseProduct.TransferredOut;
                    destinationWarehouseProduct.Modified = DateTime.Now;

                    Repository.Save();
                }
            }
        }

        public string GetOrderNumber(string shopId)
        {
            var queryable = this.Repository.Get().Where(x => x.ShopId == shopId).OrderBy(x => x.Id);
            int count = queryable.Count() + 1;

            do
            {
                var number = "T-" + count.ToString().PadLeft(7, '0');
                var prod = queryable.FirstOrDefault(x => x.OrderNumber == number) == null;
                if (prod)
                {
                    return number;
                }

                count = count + 1;
            }
            while (true);
        }
    }
}
