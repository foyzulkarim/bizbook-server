using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model.Products;
using Model.Sales;
using Model.Shops;
using RequestModel.Sales;
using ViewModel.Sales;
using BaseRepo = CommonLibrary.Repository.BaseRepository<Model.Products.ProductDetail>;
using Rm = RequestModel.Products.ProductDetailRequestModel;
using Vm = ViewModel.Products.ProductDetailViewModel;
using System;
using System.Collections.Generic;
using CommonLibrary.RequestModel;
using Model;
using ViewModel.History;
using Model.Purchases;
using Model.Warehouses;
using ReportModel;
using RequestModel.Purchases;
using ViewModel.Purchases;
using RequestModel.Products;
using RequestModel.Warehouses;
using ViewModel.Products;
using ViewModel.Reports;
using ViewModel.Warehouses;

namespace ServiceLibrary.Products
{
    using RequestModel.Reports;

    public class ProductDetailService : BaseService<ProductDetail, Rm, Vm>
    {
        public ProductDetailService(BaseRepo repository) : base(repository)
        {

        }

        public async Task<string> GetBarcode(string shopId)
        {
            var shops = await this.Repository.Db.Set<Shop>().OrderBy(x => x.Id).AsQueryable().Select(x => x.Id).ToListAsync();
            int index = shops.FindIndex(x => x == shopId);
            var queryable = this.GetQueryable(shopId);
            int count = queryable.Count() + 1;

            do
            {
                var barcode = index.ToString().PadLeft(3, '0') + count.ToString().PadLeft(6, '0');
                var prod = queryable.FirstOrDefault(x => x.BarCode == barcode) == null;
                if (prod)
                {
                    return barcode;
                }

                count = count + 1;
            } while (true);
        }

        public string GetBarcode2(string shopId)
        {
            var shopIds = this.Repository.Db.Set<Shop>().OrderBy(x => x.Id).Select(x => x.Id).ToList();
            int index = shopIds.FindIndex(x => x == shopId);
            var queryable = this.GetQueryable(shopId);
            int count = queryable.Count() + 1;

            do
            {
                var barcode = index.ToString().PadLeft(3, '0') + count.ToString().PadLeft(6, '0');
                var prod = queryable.FirstOrDefault(x => x.BarCode == barcode) == null;
                if (prod)
                {
                    return barcode;
                }

                count = count + 1;
            } while (true);
        }


        private IQueryable<ProductDetail> GetQueryable(string shopId)
        {
            IQueryable<ProductDetail> queryable = this.Repository.Get().Where(x => x.ShopId == shopId).OrderBy(x => x.Id);
            return queryable;
        }

        public async Task<Tuple<Vm, List<ProductReportViewModel>, int>> GetHistoryByDate(Rm rm)
        {
            var productDetailService = new BaseService<ProductDetail, ProductDetailRequestModel, ProductDetailViewModel>(new BaseRepository<ProductDetail>(Repository.Db));
            var productDetail = productDetailService.GetById(rm.ParentId);

            var productDetailViewModel = new Vm(productDetail);

            var saleDetailService = new BaseService<SaleDetail, SaleDetailRequestModel, SaleDetailViewModel>(new BaseRepository<SaleDetail>(Repository.Db));
            var saleDetailRequestModel = new SaleDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = -1,
                IsIncludeParents = true,
                WarehouseId = rm.WarehouseId,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate,
            };
            // this will pull all data

            Tuple<List<SaleDetailViewModel>, int> saleDetailResult = await saleDetailService.SearchAsync(saleDetailRequestModel);
            List<HistoryViewModel> saleDetailHistoryViewModels = saleDetailResult.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, productDetail.SalePrice)).ToList();

            var purchaseDetailService = new BaseService<PurchaseDetail, PurchaseDetailRequestModel, PurchaseDetailViewModel>(new BaseRepository<PurchaseDetail>(Repository.Db));
            var purchaseDetailRequestModel = new PurchaseDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = -1,
                PerPageCount = rm.PerPageCount,
                IsIncludeParents = true,
                WarehouseId = rm.WarehouseId,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate,
            };

            Tuple<List<PurchaseDetailViewModel>, int> purchaseDetailResult = await purchaseDetailService.SearchAsync(purchaseDetailRequestModel);
            List<HistoryViewModel> purchaseDetailHistoryViewModels = purchaseDetailResult.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, productDetail.CostPrice)).ToList();
            saleDetailHistoryViewModels.AddRange(purchaseDetailHistoryViewModels);
            List<HistoryViewModel> merged = saleDetailHistoryViewModels.OrderByDescending(x => x.Date).ToList();

            List<IGrouping<DateTime, HistoryViewModel>> list = merged.GroupBy(x => x.Date.Date).ToList();

            List<ProductReportViewModel> reportModels = new List<ProductReportViewModel>();
            foreach (IGrouping<DateTime, HistoryViewModel> groupModels in list)
            {
                double purchasedToday = groupModels.Where(x => x.Type == "Purchase").Sum(x => x.Quantity);
                double soldToday = groupModels.Where(x => x.Type == "Sale").Sum(x => x.Quantity);

                double soldPendingToday = groupModels.Where(x => x.Type == "Sale" && x.OrderState == OrderState.Pending)
                    .Sum(x => x.Quantity);

                double soldProcessingToday = groupModels.Where(x => x.Type == "Sale" && x.OrderState > OrderState.Pending && x.OrderState < OrderState.Delivered)
                    .Sum(x => x.Quantity);

                double soldDoneToday = groupModels.Where(x => x.Type == "Sale" && x.OrderState != OrderState.Cancel && x.OrderState >= OrderState.Delivered)
                    .Sum(x => x.Quantity);

                double purchasedAmount = groupModels.Where(x => x.Type == "Purchase").Sum(x => x.Total);
                double soldAmount = groupModels.Where(x => x.Type == "Sale").Sum(x => x.Total);

                ProductReport reportModel = new ProductReport()
                {
                    Id = productDetail.Id,
                    Date = groupModels.Key,
                    ProductDetailId = productDetail.Id,
                    IsActive = productDetail.IsActive,
                    ShopId = productDetail.ShopId,
                    ProductCategoryId = productDetail.ProductCategoryId,
                    QuantityPurchaseToday = purchasedToday,
                    QuantitySaleToday = soldToday,
                    QuantitySalePendingToday = soldPendingToday,
                    QuantitySaleProcessingToday = soldProcessingToday,
                    QuantitySaleDoneToday = soldDoneToday,
                    AmountPurchaseToday = purchasedAmount,
                    AmountSaleToday = soldAmount,
                };

                reportModels.Add(new ProductReportViewModel(reportModel));
            }

            return new Tuple<Vm, List<ProductReportViewModel>, int>(productDetailViewModel, reportModels, reportModels.Count);
        }

        public async Task<Tuple<Vm, List<HistoryViewModel>, int>> GetHistory(Rm rm)
        {
            var productDetailService = new BaseService<ProductDetail, ProductDetailRequestModel, ProductDetailViewModel>(new BaseRepository<ProductDetail>(Repository.Db));
            var productDetail = productDetailService.GetById(rm.ParentId);

            var productDetailViewModel = new Vm(productDetail);


            var service1 = new BaseService<SaleDetail, SaleDetailRequestModel, SaleDetailViewModel>(new BaseRepository<SaleDetail>(Repository.Db));
            var saleDetailRequestModel = new SaleDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = rm.Page,
                IsIncludeParents = true,
                WarehouseId = rm.WarehouseId,
                PerPageCount = rm.PerPageCount,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate
            };
            // this will pull all data

            Tuple<List<SaleDetailViewModel>, int> result = await service1.SearchAsync(saleDetailRequestModel);
            List<HistoryViewModel> viewModels = result.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, productDetail.SalePrice)).ToList();

            var stockTransferService =
                new BaseService<StockTransferDetail, StockTransferDetailRequestModel, StockTransferDetailViewModel>(
                    new BaseRepository<StockTransferDetail>(Repository.Db));
            var stockTransferDetailInRequestModel = new StockTransferDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = rm.Page,
                IsIncludeParents = true,
                DestinationWarehouseId = rm.WarehouseId,
                PerPageCount = rm.PerPageCount,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate
            };
            var stockInResult = await stockTransferService.SearchAsync(stockTransferDetailInRequestModel);
            var stockInViewModels = stockInResult.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, "StockIn")).ToList();

            var stockTransferDetailOutRequestModel = new StockTransferDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = rm.Page,
                IsIncludeParents = true,
                SourceWarehouseId = rm.WarehouseId,
                PerPageCount = rm.PerPageCount,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate
            };
            var stockOutResult = await stockTransferService.SearchAsync(stockTransferDetailOutRequestModel);
            var stockOutViewModels = stockOutResult.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, "StockOut")).ToList();

            var purchaseDetailService = new BaseService<PurchaseDetail, PurchaseDetailRequestModel, PurchaseDetailViewModel>(new BaseRepository<PurchaseDetail>(Repository.Db));
            var purchaseDetailRequestModel = new PurchaseDetailRequestModel("")
            {
                ShopId = rm.ShopId,
                ProductDetailId = rm.ParentId,
                Page = rm.Page,
                PerPageCount = rm.PerPageCount,
                IsIncludeParents = true,
                WarehouseId = rm.WarehouseId,
                StartDate = rm.StartDate,
                EndDate = rm.EndDate
            };

            Tuple<List<PurchaseDetailViewModel>, int> purchaseDetailResult = await purchaseDetailService.SearchAsync(purchaseDetailRequestModel);
            List<HistoryViewModel> models = purchaseDetailResult.Item1.ConvertAll(x => new HistoryViewModel(x, productDetail.Name, productDetail.CostPrice)).ToList();


            viewModels.AddRange(models);
            viewModels.AddRange(stockInViewModels);
            viewModels.AddRange(stockOutViewModels);
            List<HistoryViewModel> merged = viewModels.OrderByDescending(x => x.Date.Date).ToList();

            double purchased = merged.Where(x=>x.Type=="Purchase").Sum(x => x.Quantity);
            double sold = merged
                .Where(x => x.Type == "Sale" && x.OrderState > OrderState.Pending && x.OrderState < OrderState.Cancel)
                .Sum(x => x.Quantity);
            double stockInQuantity = merged
                .Where(x => x.Type == "StockIn" && x.TransferState == StockTransferState.Approved.ToString())
                .Sum(x => x.Quantity);
            double stockOutQuantity = merged
                .Where(x => x.Type == "StockOut" && x.TransferState == StockTransferState.Approved.ToString())
                .Sum(x => x.Quantity);

            productDetailViewModel.Purchased = purchased;
            productDetailViewModel.Sold = sold;
            productDetailViewModel.StockIn = stockInQuantity;
            productDetailViewModel.StockOut = stockOutQuantity;
            productDetailViewModel.OnHand = purchased
                                            + stockInQuantity
                                            - sold
                                            - stockOutQuantity;

            return new Tuple<Vm, List<HistoryViewModel>, int>(productDetailViewModel, merged, viewModels.Count);
        }

        public async Task<Tuple<List<HistoryViewModel>, int>> GetProductHistoryByCustomer(Rm rm)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            List<SaleDetail> models = await db.SaleDetails.Include(x => x.Sale).Include(x => x.ProductDetail).Where(x => x.ShopId == rm.ShopId && x.Sale.CustomerId == rm.ParentId)
                                          .ToListAsync();
            List<SaleDetailViewModel> viewModels = models.ConvertAll(x => new SaleDetailViewModel(x)).ToList();
            List<HistoryViewModel> historyViewModels = viewModels.ConvertAll(x => new HistoryViewModel(x, x.ProductDetailName, x.SalePricePerUnit)).ToList();
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

         
        public string GetProductCode(string detailName)
        {
            string[] separator = new[] { " ", "-" };
            string[] strings = detailName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string c = "";
            foreach (string s in strings)
            {
                c += s[0].ToString();
            }

            return c;
        }


        public ProductGroup GetDefaultProductGroup(BusinessDbContext db, string shopId, string username)
        {
            ProductGroup productGroup = db.ProductGroups.FirstOrDefault(x => x.Name == "General" && x.ShopId == shopId);
            if (productGroup == null)
            {
                productGroup = new ProductGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    CreatedBy = username,
                    ModifiedBy = username,
                    ShopId = shopId,
                    CreatedFrom = "Server",
                    Name = "General",
                };
                db.ProductGroups.Add(productGroup);
                db.SaveChanges();
            }

            return productGroup;
        }

        public async Task<Tuple<List<ProductDetailViewModel>, int>> SearchByWarehouseAsync(ProductDetailRequestModel request)
        {
            BusinessDbContext db = base.Repository.Db as BusinessDbContext;
            var tuple = await base.SearchAsync(request);
            var whProducts = db.WarehouseProducts.Where(x => x.ShopId == request.ShopId && x.WarehouseId == request.WarehouseId);
            var enumerable = from m in tuple.Item1
                             join wh in whProducts on m.Id equals wh.ProductDetailId into ps
                             from wh in ps.DefaultIfEmpty()
                             select new { ProductDetail = m, Quantity = wh?.OnHand ?? 0 };
            foreach (var v in enumerable)
            {
                v.ProductDetail.OnHand = v.Quantity;
            }

            var list = enumerable.Select(x => x.ProductDetail).ToList();
            var tuple1 = new Tuple<List<ProductDetailViewModel>, int>(list, tuple.Item2);
            return tuple1;
        }

        public Tuple<List<ProductReport>, int> GetProductStockReport(ProductReportRequestModel request)
        {
            BusinessDbContext db = base.Repository.Db as BusinessDbContext;
            request.StartDate = request.StartDate.Date;
            request.EndDate = request.EndDate.Date.AddDays(1).AddMinutes(-1);

            var dbSaleDetailsDone = db.SaleDetails.Include(x => x.Sale).Where(x => x.Sale.OrderState == OrderState.Delivered || x.Sale.OrderState == OrderState.Completed).AsQueryable();
            var dbSaleDetailsProcessing = db.SaleDetails.Include(x => x.Sale).Where(x => x.Sale.OrderState > OrderState.Pending && x.Sale.OrderState < OrderState.Delivered).AsQueryable();
            var dbPurchaseDetails = db.PurchaseDetails.AsQueryable();
            var dbStockTransferOutDetailsApproved = db.StockTransferDetails.Include(x => x.StockTransfer).Where(x=>x.StockTransfer.TransferState== StockTransferState.Approved).AsQueryable();
            var dbStockTransferInDetailsApproved = db.StockTransferDetails.Include(x => x.StockTransfer).Where(x => x.StockTransfer.TransferState == StockTransferState.Approved).AsQueryable();
            var dbStockTransferOutDetailsPending = db.StockTransferDetails.Include(x => x.StockTransfer).Where(x => x.StockTransfer.TransferState == StockTransferState.Pending).AsQueryable();
            var dbStockTransferInDetailsPending = db.StockTransferDetails.Include(x => x.StockTransfer).Where(x => x.StockTransfer.TransferState == StockTransferState.Pending).AsQueryable();

            if (request.WarehouseId.IdIsOk() && request.WarehouseId != new Guid().ToString())
            {
                dbSaleDetailsDone = dbSaleDetailsDone.Where(x => x.WarehouseId == request.WarehouseId);
                dbSaleDetailsProcessing = dbSaleDetailsProcessing.Where(x => x.WarehouseId == request.WarehouseId);
                dbPurchaseDetails = dbPurchaseDetails.Where(x => x.WarehouseId == request.WarehouseId);
                dbStockTransferOutDetailsApproved =
                    dbStockTransferOutDetailsApproved.Where(x => x.SourceWarehouseId == request.WarehouseId);
                dbStockTransferInDetailsApproved =
                    dbStockTransferInDetailsApproved.Where(x => x.DestinationWarehouseId == request.WarehouseId);
            }

            List<StockReportModelTemp> purchaseDetailListBefore = dbPurchaseDetails
                .Where(x => x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) < request.StartDate)
                .GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.Paid),
                    RowsCount = x.Select(y => y.PurchaseId).Distinct().Count()
                }).ToList();

            List<StockReportModelTemp> saleDetailListBeforeDone = dbSaleDetailsDone.Where(x =>
                x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.PaidAmount),
                    RowsCount = x.Select(y => y.SaleId).Distinct().Count()
                }).ToList();
            List<StockReportModelTemp> saleDetailListBeforeProcessing = dbSaleDetailsProcessing.Where(x =>
                x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.PaidAmount),
                    RowsCount = x.Select(y => y.SaleId).Distinct().Count()
                }).ToList();

            List<StockReportModelTemp> purchaseDetailList = dbPurchaseDetails
                .Where(x => x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) >= request.StartDate && DbFunctions.TruncateTime(x.Created) <= request.EndDate)
                .GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.Paid),
                    RowsCount = x.Select(y => y.PurchaseId).Distinct().Count()
                }).ToList();

            List<StockReportModelTemp> saleDetailListDone = dbSaleDetailsDone.Where(x =>
                x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) >= request.StartDate && DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.PaidAmount),
                    RowsCount = x.Select(y => y.SaleId).Distinct().Count()
                }).ToList();

            List<StockReportModelTemp> saleDetailListProcessing = dbSaleDetailsProcessing.Where(x =>
                x.ShopId == request.ShopId && DbFunctions.TruncateTime(x.Created) >= request.StartDate && DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId).Select(x => new StockReportModelTemp()
                {
                    ProductDetailId = x.Key,
                    Quantity = x.Sum(y => y.Quantity),
                    Amount = x.Sum(y => y.PaidAmount),
                    RowsCount = x.Select(y => y.SaleId).Distinct().Count()
                }).ToList();

            List<StockReportModelTemp> stockTransferInDetailListBeforeApproved = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferInDetailListApproved = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferOutDetailListBeforeApproved = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferOutDetailListApproved = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferInDetailListBeforePending = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferInDetailListPending = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferOutDetailListBeforePending = new List<StockReportModelTemp>();
            List<StockReportModelTemp> stockTransferOutDetailListPending = new List<StockReportModelTemp>();

            if (request.WarehouseId.IdIsOk() && request.WarehouseId != new Guid().ToString())
            {
               
                stockTransferInDetailListBeforeApproved = dbStockTransferInDetailsApproved
                    .Where(x => x.ShopId == request.ShopId &&
                                x.DestinationWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId)
               .Select(x => new StockReportModelTemp()
               {
                   ProductDetailId = x.Key,
                   Quantity = x.Sum(y => y.Quantity),
                   Amount = x.Sum(y => y.PriceTotal),
                   RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
               }).ToList();

              stockTransferOutDetailListBeforeApproved = dbStockTransferOutDetailsApproved
                    .Where(x => x.ShopId == request.ShopId &&
                                x.SourceWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId)
                    .Select(x => new StockReportModelTemp()
                    {
                        ProductDetailId = x.Key,
                        Quantity = x.Sum(y => y.Quantity),
                        Amount = x.Sum(y => y.PriceTotal),
                        RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                    }).ToList();

                stockTransferInDetailListApproved = dbStockTransferInDetailsApproved
                    .Where(x => x.ShopId == request.ShopId &&
                                x.DestinationWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) >= request.StartDate &&
                                DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId)
                    .Select(x => new StockReportModelTemp()
                    {
                        ProductDetailId = x.Key,
                        Quantity = x.Sum(y => y.Quantity),
                        Amount = x.Sum(y => y.PriceTotal),
                        RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                    }).ToList();

                stockTransferOutDetailListApproved = dbStockTransferOutDetailsApproved
                    .Where(x => x.ShopId == request.ShopId &&
                                x.SourceWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) >= request.StartDate &&
                                DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId)
                    .Select(x => new StockReportModelTemp()
                    {
                        ProductDetailId = x.Key,
                        Quantity = x.Sum(y => y.Quantity),
                        Amount = x.Sum(y => y.PriceTotal),
                        RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                    }).ToList();

                stockTransferInDetailListBeforePending = dbStockTransferInDetailsPending
                    .Where(x => x.ShopId == request.ShopId &&
                                x.DestinationWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId)
               .Select(x => new StockReportModelTemp()
               {
                   ProductDetailId = x.Key,
                   Quantity = x.Sum(y => y.Quantity),
                   Amount = x.Sum(y => y.PriceTotal),
                   RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
               }).ToList();

                stockTransferOutDetailListBeforePending = dbStockTransferOutDetailsPending
                      .Where(x => x.ShopId == request.ShopId &&
                                  x.SourceWarehouseId == request.WarehouseId &&
                                  DbFunctions.TruncateTime(x.Created) < request.StartDate).GroupBy(x => x.ProductDetailId)
                      .Select(x => new StockReportModelTemp()
                      {
                          ProductDetailId = x.Key,
                          Quantity = x.Sum(y => y.Quantity),
                          Amount = x.Sum(y => y.PriceTotal),
                          RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                      }).ToList();

                stockTransferInDetailListPending = dbStockTransferInDetailsPending
                    .Where(x => x.ShopId == request.ShopId &&
                                x.DestinationWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) >= request.StartDate &&
                                DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId)
                    .Select(x => new StockReportModelTemp()
                    {
                        ProductDetailId = x.Key,
                        Quantity = x.Sum(y => y.Quantity),
                        Amount = x.Sum(y => y.PriceTotal),
                        RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                    }).ToList();

                stockTransferOutDetailListPending = dbStockTransferOutDetailsPending
                    .Where(x => x.ShopId == request.ShopId &&
                                x.SourceWarehouseId == request.WarehouseId &&
                                DbFunctions.TruncateTime(x.Created) >= request.StartDate &&
                                DbFunctions.TruncateTime(x.Created) <= request.EndDate).GroupBy(x => x.ProductDetailId)
                    .Select(x => new StockReportModelTemp()
                    {
                        ProductDetailId = x.Key,
                        Quantity = x.Sum(y => y.Quantity),
                        Amount = x.Sum(y => y.PriceTotal),
                        RowsCount = x.Select(y => y.StockTransferId).Distinct().Count()
                    }).ToList();
            }

            var productDetails = db.ProductDetails.AsQueryable();
            if (request.IsProductActive)
            {
                productDetails = productDetails.Where(x => x.IsActive);
            }

            var products = productDetails.Where(x => x.ShopId == request.ShopId).Include(x => x.ProductCategory)
                .Include(x => x.ProductCategory.ProductGroup).ToList().OrderBy(x => x.Name);

            List<ProductReport> productReports = new List<ProductReport>();

            foreach (var productDetail in products)
            {
                StockReportModelTemp purchaseDetail = purchaseDetailList.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp saleDetailDone = saleDetailListDone.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp saleDetailProcessing = saleDetailListProcessing.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                
                StockReportModelTemp purchaseDetailBefore =
                    purchaseDetailListBefore.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp saleDetailBeforeDone = saleDetailListBeforeDone.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp saleDetailBeforeProcessing = saleDetailListBeforeProcessing.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);

                StockReportModelTemp stockInApproved = stockTransferInDetailListApproved.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp stockOutApproved = stockTransferOutDetailListApproved.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);

                StockReportModelTemp stockInPending = stockTransferInDetailListPending.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);
                StockReportModelTemp stockOutPending = stockTransferOutDetailListPending.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);

                StockReportModelTemp stockInBeforeApproved = stockTransferInDetailListBeforeApproved.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);

                StockReportModelTemp stockOutBeforeApproved = stockTransferOutDetailListBeforeApproved.FirstOrDefault(x => x.ProductDetailId == productDetail.Id);

                var purchaseQuantityBefore = purchaseDetailBefore?.Quantity ?? 0;
                var stockInQuantityBeforeApproved = stockInBeforeApproved?.Quantity ?? 0;
                var stockOutQuantityBeforeApproved = stockOutBeforeApproved?.Quantity ?? 0;
                var saleQuantityBeforeDone = saleDetailBeforeDone?.Quantity ?? 0;
                var saleQuantityBeforeProcessing = saleDetailBeforeProcessing?.Quantity ?? 0;
                var startingToday = stockInQuantityBeforeApproved 
                                    + purchaseQuantityBefore 
                                    - saleQuantityBeforeProcessing 
                                    - saleQuantityBeforeDone 
                                    - stockOutQuantityBeforeApproved;

                var productReport = new ProductReport();
                productReport.Id = productDetail.Id;
                productReport.ProductDetailId = productDetail.Id;
                productReport.ProductDetailName = productDetail.Name;
                productReport.ProductCategoryId = productDetail.ProductCategoryId;
                productReport.ProductCategoryName = productDetail.ProductCategory.Name;
                productReport.ProductGroupId = productDetail.ProductCategory.ProductGroupId;
                productReport.ProductGroupName = productDetail.ProductCategory.ProductGroup.Name;
                productReport.QuantityStartingToday = startingToday;
                productReport.QuantityPurchaseToday = purchaseDetail?.Quantity ?? 0;
                productReport.QuantitySaleDoneToday = saleDetailDone?.Quantity ?? 0;
                productReport.QuantitySaleProcessingToday = saleDetailProcessing?.Quantity ?? 0;
                productReport.QuantityStockInApprovedToday = stockInApproved?.Quantity ?? 0;
                productReport.QuantityStockOutApprovedToday = stockOutApproved?.Quantity ?? 0;
                productReport.QuantityStockInPendingToday = stockInPending?.Quantity ?? 0;
                productReport.QuantityStockOutPendingToday = stockOutPending?.Quantity ?? 0;
                productReport.AmountSaleToday = saleDetailDone?.Amount ?? 0;
                productReport.RowsCount = saleDetailDone?.RowsCount ?? 0;
                productReport.ShopId = productDetail.ShopId;
                productReport.Created = request.StartDate;
                productReport.IsActive = productDetail.IsActive;

                productReport.QuantityEndingToday = 
                    startingToday 
                    + productReport.QuantityPurchaseToday 
                    + productReport.QuantityStockInApprovedToday
                    - productReport.QuantitySaleToday 
                    - productReport.QuantityStockOutApprovedToday;

                productReports.Add(productReport);
            }

            //var reportViewModels = productReports.ConvertAll(x => new ProductReportViewModel(x)).ToList();

            return new Tuple<List<ProductReport>, int>(productReports, productReports.Count);
        }
    }
}