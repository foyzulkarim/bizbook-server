using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using ViewModel.Reports;

namespace ServiceLibrary.Reports
{
    using System.Data.Entity;
    using Model;
    using Model.Products;
    using ReportModel;

    using RequestModel.Reports;

    public class ProductReportService2 : BaseReportService, IReportService2
    {
        public string QuickUpdate(string shopId, string itemId, DateTime date)
        {
            date = date.Date;
            ReportDbContext rDb = new ReportDbContext();
            BusinessDbContext bDb = new BusinessDbContext();

            ProductReport report = rDb.ProductReports.FirstOrDefault(
                x => x.ShopId == shopId && x.ProductDetailId == itemId && x.Date == date);
            if (report == null)
            {
                report = GetReportObject(itemId, bDb);

                this.SetDefaults(report, shopId, date);
                rDb.ProductReports.Add(report);
                rDb.SaveChanges();
                report = rDb.ProductReports.First(x => x.Id == report.Id);
            }

            var saleDetails =
                bDb.SaleDetails
                .Where(x => x.ShopId == shopId
                            && DbFunctions.TruncateTime(x.Created) == date)
                .Where(x => x.ProductDetailId == itemId).AsQueryable();
            var purchaseDetails =
                bDb.PurchaseDetails
                .Where(x => x.ShopId == shopId
                            && DbFunctions.TruncateTime(x.Created) == date)
                .Where(x => x.ProductDetailId == itemId).AsQueryable();

            var saleListToday = saleDetails.ToList();
            var purchaseListToday = purchaseDetails.ToList();

            // quantities 
            report.QuantityPurchaseToday = purchaseListToday.Sum(x => x.Quantity);
            report.QuantitySaleToday = saleListToday.Sum(x => x.Quantity);
            report.QuantityEndingToday =
                report.QuantityStartingToday + report.QuantityPurchaseToday - report.QuantitySaleToday;

            // sale amounts
            report.AmountSaleToday = saleListToday.Sum(x => x.Total);
            report.AmountCostForSaleToday = saleListToday.Sum(x => x.CostTotal);
            report.AmountReceivedToday = saleListToday.Sum(x => x.PaidAmount);
            report.AmountReceivableToday = saleListToday.Sum(x => x.DueAmount);
            if (report.QuantitySaleToday > 0)
            {
                report.AmountAverageSalePriceToday = report.AmountSaleToday / report.QuantitySaleToday;
            }

            report.AmountProfitToday = report.AmountSaleToday - report.AmountCostForSaleToday;

            if (report.AmountCostForSaleToday > 0)
            {
                report.AmountProfitPercentToday = report.AmountProfitToday * 100 / report.AmountCostForSaleToday;
            }

            // purchase amounts
            report.AmountPurchaseToday = purchaseListToday.Sum(x => x.CostTotal);
            report.AmountPaidToday = purchaseListToday.Sum(x => x.Paid);
            report.AmountPayableToday = purchaseListToday.Sum(x => x.Payable);

            if (report.QuantityPurchaseToday > 0)
            {
                report.AmountAveragePurchasePricePerUnitToday = report.AmountPurchaseToday / report.QuantityPurchaseToday;
            }
            
            report.Modified = DateTime.Now;
            int i = rDb.SaveChanges();
            return report.Id;
        }
        
        public bool DayEndUpdate(string shopId, string itemId, DateTime date)
        {
            date = date.Date;
            ReportDbContext rDb = new ReportDbContext();
            BusinessDbContext bDb = new BusinessDbContext();
            var report = rDb.ProductReports.FirstOrDefault(
                x => x.ShopId == shopId && x.ProductDetailId == itemId && x.Date == date);
            if (report == null)
            {
                string id = this.QuickUpdate(shopId, itemId, date);
                report = rDb.ProductReports.Find(id);
            }

            var saleDetailsAllProduct = bDb.SaleDetails
                .Where(x => x.ShopId == shopId
                            && DbFunctions.TruncateTime(x.Modified) == date);

            var purchaseDetailsAllProduct = bDb.PurchaseDetails
                .Where(x => x.ShopId == shopId
                            && DbFunctions.TruncateTime(x.Modified) == date);

            var allProductPurchase = purchaseDetailsAllProduct.Select(x => new { x.Quantity, x.CostTotal, x.Paid }).ToList();
            var allProductSale = saleDetailsAllProduct.Select(
                x => new { x.Quantity, x.Total, x.CostTotal, x.Created, Profit = (x.Total - x.CostTotal) }).ToList();

            if (allProductPurchase.Sum(x => x.Quantity) > 0)
            {
                report.QuantityPurchasePercentInAllProductsToday =
                    report.QuantityPurchaseToday * 100 / allProductPurchase.Sum(x => x.Quantity);
            }

            if (allProductPurchase.Sum(x => x.CostTotal) > 0)
            {
                report.AmountPurchasePercentInAllProductsToday =
                    report.AmountPurchaseToday * 100 / allProductPurchase.Sum(x => x.CostTotal);
            }

            if (allProductSale.Count > 0)
            {
                report.QuantitySalePercentInAllProductsToday =
                    allProductSale.Sum(x => x.Quantity) > 0
                        ? report.QuantitySaleToday * 100 / allProductSale.Sum(x => x.Quantity)
                        : 0;
                report.AmountSalePercentInAllProductsToday =
                    allProductSale.Sum(x => x.Total) > 0
                        ? report.AmountSaleToday * 100 / allProductSale.Sum(x => x.Total)
                        : 0;

                report.AmountProfitPercentInAllProductsToday =
                    allProductSale.Sum(x => x.Profit) > 0 ?
                    report.AmountProfitToday * 100 / allProductSale.Sum(x => x.Profit) : 0;
            }

            var saleDetailsWithSaleByProduct = saleDetailsAllProduct.Include(x => x.Sale).Where(x => x.ProductDetailId == itemId).ToList();
            var dealerSales = saleDetailsWithSaleByProduct.Where(x => x.Sale.IsDealerSale).ToList();
            var customerSales = saleDetailsWithSaleByProduct.Where(x => x.Sale.IsDealerSale == false).ToList();

            report.QuantitySaleToDealerToday = dealerSales.Sum(x => x.Quantity);
            report.AmountSaleToDealerToday = dealerSales.Sum(x => x.Total);
            report.QuantitySaleToCustomerToday = customerSales.Sum(x => x.Quantity);
            report.AmountSaleToCustomerToday = customerSales.Sum(x => x.Total);

            return rDb.SaveChanges() > 0;
        }

        public bool DayEndUpdateAll(string shopId, DateTime date)
        {
            date = date.Date;
            BusinessDbContext bDb = new BusinessDbContext();
            List<string> ids = bDb.ProductDetails.Where(x => x.ShopId == shopId).Select(x => x.Id).ToList();
            foreach (string id in ids)
            {
                QuickUpdate(shopId, id, date);
            }

            foreach (var id in ids)
            {
                DayEndUpdate(shopId, id, date);
            }

            return true;
        }

        public bool DayStartAll(string shopId, DateTime startDate)
        {
            startDate = startDate.Date;
            DateTime yesterday = startDate.AddDays(-1).Date;
         
            DayEndUpdateAll(shopId, yesterday);

            ReportDbContext rDb = new ReportDbContext();
            BusinessDbContext bDb = new BusinessDbContext();
            List<string> ids = bDb.ProductDetails.Where(x => x.ShopId == shopId).Select(x => x.Id).ToList();

            foreach (string pId in ids)
            {
                ProductReport report = GetReportObject(pId, bDb);
                this.SetDefaults(report, shopId, startDate);

                ProductReport yesterdayReport = rDb.ProductReports.Where(x => x.ShopId == shopId && x.ProductDetailId == pId).FirstOrDefault(x => DbFunctions.TruncateTime(x.Date) == yesterday);
                if (yesterdayReport != null)
                {
                    report.QuantityStartingToday = yesterdayReport.QuantityEndingToday;
                }
                var todayExists = rDb.ProductReports.Any(
                    x => x.ShopId == shopId && x.ProductDetailId == pId && DbFunctions.TruncateTime(x.Date) == startDate);
                if (todayExists)
                {
                    this.QuickUpdate(shopId, pId, startDate);
                }
                else
                {
                    rDb.ProductReports.Add(report);
                    rDb.SaveChanges();
                }                
            }

            return true;
        }
        
        public async Task<Tuple<List<ProductReportViewModel>, int>> SearchAsync(ProductReportRequestModel request)
        {
            ReportDbContext db = new ReportDbContext();
            BaseRepository<ProductReport> repo = new BaseRepository<ProductReport>(db);
            var service = new BaseService<ProductReport, ProductReportRequestModel, ProductReportViewModel>(repo);
            Tuple<List<ProductReportViewModel>, int> tuple = await service.SearchAsync(request);
            return tuple;
        }

        private static ProductReport GetReportObject(string itemId, BusinessDbContext bDb)
        {
            ProductDetail productDetail = bDb.ProductDetails.Where(x => x.Id == itemId).Include(x => x.ProductCategory)
                .Include(x => x.ProductCategory.ProductGroup).First();

            var report = new ProductReport
                             {
                                 ProductDetailId = itemId,
                                 ProductDetailName = productDetail.Name,
                                 ProductCategoryId = productDetail.ProductCategoryId,
                                 ProductCategoryName = productDetail.ProductCategory.Name,
                                 ProductGroupId = productDetail.ProductCategory.ProductGroupId,
                                 ProductGroupName = productDetail.ProductCategory.ProductGroup.Name,
                             };
            return report;
        }
    }
}
