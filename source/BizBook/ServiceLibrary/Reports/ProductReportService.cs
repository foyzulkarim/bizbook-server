using Model.Products;
using T = Model.Products.ProductDetail;
using Vm = ViewModel.Reports.ProductReportViewModel;
using M = ReportModel.ProductReport;
using Rm = RequestModel.Reports.ProductReportRequestModel;
using Repo = CommonLibrary.Repository.BaseRepository<ReportModel.ProductReport>;

namespace ServiceLibrary.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using CommonLibrary.Service;

    using Model;
    using Model.Purchases;
    using Model.Sales;

    using ReportModel;
    using ReportModel.Parameters;

    public class ProductReportService : BaseService<M, Rm,Vm>
    {
        protected Expression<Func<SaleDetail, int?>> yearKeySelector = x => SqlFunctions.DatePart("year", x.Modified);
        
        public void ShopClose(string shopId, DateTime start, DateTime end, ReportTimeType reportTimeType)
        {
            DateTime starts = DateTime.Now;
            var db = BusinessDbContext.Create();
            List<ProductDetail> productDetails = db.ProductDetails.Where(x => x.ShopId == shopId).AsQueryable().ToList();
            List<ProductCategory> categories = db.ProductCategories.Where(x => x.ShopId == shopId)
                .Include(x => x.ProductGroup).ToList();

            var purchaseDetails = db.PurchaseDetails
                .Where(x => x.ShopId == shopId  && x.Modified >= start && x.Modified <= end)
                .OrderBy(x => x.Modified).AsQueryable().ToList();

            var saleDetails = db.SaleDetails
                .Where(x => x.ShopId == shopId  && x.Modified >= start && x.Modified <= end)
                .OrderBy(x => x.Modified).AsQueryable().ToList();

            var purchaseDates = purchaseDetails.Select(x => x.Modified.Date).Distinct().OrderBy(x => x).ToList();
            var saleDates = saleDetails.Select(x => x.Modified.Date).Distinct().OrderBy(x => x).ToList();
            var alldates = purchaseDates.Union(saleDates).OrderBy(x => x).ToList();

            //foreach (DateTime operationDate in alldates)
            //{
            //    this.UpsertDailyAllProduct(shopId, reportTimeType, operationDate, purchaseDetails, saleDetails, productDetails, categories);
            //}

            Parallel.ForEach(
                alldates,
                operationDate => this.UpsertDailyAllProduct(
                    shopId,
                    reportTimeType,
                    operationDate,
                    purchaseDetails,
                    saleDetails,
                    productDetails,
                    categories));

            DateTime ends = DateTime.Now;
            Console.WriteLine("Completed in " + (ends - starts).TotalSeconds);
            Console.Read();
        }

        private void UpsertDailyAllProduct(
            string shopId,
            ReportTimeType reportTimeType,
            DateTime operationDate,
            List<PurchaseDetail> purchaseDetails,
            List<SaleDetail> saleDetails,
            List<ProductDetail> productDetails,
            List<ProductCategory> categories)
        {
            Console.WriteLine($"Operation date : {operationDate}");
            var dailyPurchase = purchaseDetails.Where(x => x.Modified.Date == operationDate);
            var dailySales = saleDetails.Where(x => x.Modified.Date == operationDate);

            List<IGrouping<string, PurchaseDetail>> purchaseDetailsGrouped =
                dailyPurchase.GroupBy(x => x.ProductDetailId).ToList();
            List<IGrouping<string, SaleDetail>> saleDetailsGrouped = dailySales.GroupBy(x => x.ProductDetailId).ToList();

            //List<string> dailyPurchaseIds = purchaseDetailsGrouped.Select(x => x.Key).ToList();
            //List<string> dailySoldIds = saleDetailsGrouped.Select(x => x.Key).ToList();
            //List<string> dailyAllIds = dailySoldIds.Union(dailyPurchaseIds).ToList();
            List<string> dailyAllIds = productDetails.Select(x => x.Id).Distinct().ToList();


            string value = this.GetValue(reportTimeType, operationDate);
            ReportParameterBase reportParameterBase = new ReportParameterBase(operationDate, value, shopId);

            //foreach (string productDetailId in dailyAllIds)
            //{
            //    this.UpsertDailyProductReport(productDetailId, reportParameterBase, saleDetailsGrouped, purchaseDetailsGrouped, productDetails, categories);
            //}

            Parallel.ForEach(
                dailyAllIds,
                productDetailId => this.UpsertDailyProductReport(
                    productDetailId,
                    reportParameterBase,
                    saleDetailsGrouped,
                    purchaseDetailsGrouped,
                    productDetails,
                    categories));
        }

        private void UpsertDailyProductReport(
            string productDetailId,
            ReportParameterBase reportParameterBase,
            List<IGrouping<string, SaleDetail>> saleDetailsGrouped,
            List<IGrouping<string, PurchaseDetail>> purchaseDetailsGrouped,
            List<ProductDetail> productDetails,
            List<ProductCategory> categories)
        {
            ProductReportParameter parameter = new ProductReportParameter(
                reportParameterBase.Date,
                reportParameterBase.Value,
                reportParameterBase.ReportShopId);
            parameter.StartingQuantity = this.GetPrevious(parameter);

            var sold = saleDetailsGrouped.Exists(x => x.Key == productDetailId)
                           ? saleDetailsGrouped.First(x => x.Key == productDetailId).ToList()
                           : new List<SaleDetail>();
            var purchased = purchaseDetailsGrouped.Exists(x => x.Key == productDetailId)
                                ? purchaseDetailsGrouped.First(x => x.Key == productDetailId).ToList()
                                : new List<PurchaseDetail>();

            T productDetail = productDetails.First(x => x.Id == productDetailId);
            var category = categories.First(x => x.Id == productDetail.ProductCategoryId);
            var productReport = this.CreateModel(parameter, category, productDetail, purchased, sold);
            Console.WriteLine(
                $"Model saving - {productReport.Date} and product {productReport.ProductDetailName} and SaleQuantity {productReport.QuantitySaleToday}");
            this.SaveReport(productReport);
        }

        private string GetValue(ReportTimeType reportTimeType, DateTime end)
        {
            switch (reportTimeType)
            {
                case ReportTimeType.Daily:
                    return end.ToString("dd-MMMM-yyyy");
                case ReportTimeType.Weekly:
                    Calendar calendar = new GregorianCalendar();
                    string value = "Week " + calendar.GetWeekOfYear(
                                                   end,
                                                   CalendarWeekRule.FirstDay,
                                                   DayOfWeek.Sunday) + "-" + end.Year;
                    return value;
                case ReportTimeType.Monthly:
                    return end.ToString("MMMM-yyyy");
                case ReportTimeType.Yearly:
                    return end.ToString("yyyy");
                default:
                    throw new ArgumentOutOfRangeException(nameof(reportTimeType), reportTimeType, null);
            }
        }

        private M CreateModel(ProductReportParameter parameter, ProductCategory category,
            T productDetail, List<PurchaseDetail> purchaseDetails, List<SaleDetail> saleDetails)
        {
            double purchaseTotal = purchaseDetails.Sum(x => x.CostTotal);
            double purchaseQuantity = purchaseDetails.Sum(x => x.Quantity);
            var saleCostTotal = saleDetails.Sum(x => x.CostTotal);
            double saleQuantity = saleDetails.Sum(x => x.Quantity);
            double salePriceTotal = saleDetails.Sum(x => x.PriceTotal);
            double startingQuantity = parameter.StartingQuantity;
            M productReport = new M();
            productReport.Id = Guid.NewGuid().ToString();
            productReport.CreatedFrom = "BizBook";
            productReport.CreatedBy = "Automatic";
            productReport.ModifiedBy = "Automatic";
            productReport.Created = DateTime.Today.Date;
            productReport.Modified = DateTime.Today.Date;
            productReport.ShopId = parameter.ReportShopId;
           // productReport.ReportTimeType = ReportTimeType.Daily;
            productReport.Date = parameter.Date.Date;
            productReport.Value = parameter.Date.Date.ToString("dd-MMMM-yyyy");
            productReport.RowsCount = 0;
          //  productReport.ProductReportType = ProductReportType.ProductDetailByAmount;
            productReport.ProductDetailId = productDetail.Id;
            productReport.ProductCategoryId = category.Id;
            productReport.ProductGroupId = category.ProductGroupId;
            productReport.ProductDetailName = productDetail.Name;
            productReport.ProductCategoryName = category.Name;
            productReport.ProductGroupName = category.ProductGroup.Name;

            //productReport.PurchaseTotal = purchaseTotal;
            //productReport.PurchaseQuantity = purchaseQuantity;
            //productReport.PurchasePricePerUnitAverage = purchaseQuantity == 0 ? 0 : purchaseTotal / purchaseQuantity;
            //productReport.SaleTotal = saleDetails.Sum(x => x.Total);
            //productReport.SaleCostTotal = saleCostTotal;
            //productReport.SalePriceTotal = salePriceTotal;
            //productReport.SaleDiscountTotal = saleDetails.Sum(x => x.DiscountTotal);
            //productReport.SaleQuantity = saleQuantity;
            //productReport.SaleCostPricePerUnitAverage = saleQuantity == 0 ? 0 : saleCostTotal / saleQuantity;
            //productReport.SalePricePerUnitAverage = saleQuantity == 0 ? 0 : salePriceTotal / saleQuantity;
            //productReport.StartingQuantity = startingQuantity;
            //productReport.EndingQuantity = startingQuantity + purchaseQuantity - saleQuantity;

            return productReport;
        }

        public void SaveReports(List<M> reports)
        {
            foreach (M report in reports)
            {
                SaveReport(report);
            }
        }

        private void SaveReport(ProductReport report)
        {
            var db = BusinessDbContext.Create();
            //M dbReport = db.ProductReports.FirstOrDefault(
            //    x => x.ShopId == report.ShopId
            //         && x.ReportTimeType == report.ReportTimeType
            //         && x.Date == report.Date
            //         && x.ProductReportType == report.ProductReportType
            //         && x.ProductDetailId == report.ProductDetailId);

            //if (dbReport == null)
            //{
            //    db.ProductReports.Add(report);
            //}
            //else
            //{
            //    dbReport.PurchaseTotal = report.PurchaseTotal;
            //    dbReport.PurchaseQuantity = report.PurchaseQuantity;
            //    dbReport.PurchasePricePerUnitAverage =
            //        report.PurchaseQuantity == 0 ? 0 : report.PurchaseTotal / report.PurchaseQuantity;

            //    dbReport.SaleTotal = report.SaleTotal;
            //    dbReport.SaleCostTotal = report.SaleCostTotal;
            //    dbReport.SalePriceTotal = report.SalePriceTotal;
            //    dbReport.SaleDiscountTotal = report.SaleDiscountTotal;
            //    dbReport.SaleQuantity = report.SaleQuantity;
            //    dbReport.SaleCostPricePerUnitAverage = report.SaleCostPricePerUnitAverage;
            //    dbReport.SalePricePerUnitAverage = report.SalePricePerUnitAverage;

            //    dbReport.StartingQuantity = report.StartingQuantity;
            //    dbReport.EndingQuantity = report.EndingQuantity;
            //}

            db.SaveChanges();
        }

        private double GetPrevious(ProductReportParameter parameter)
        {
            //var db = BusinessDbContext.Create();
            //var date = parameter.Date.Date;
            //var report = db.ProductReports.Where(
            //    x => x.ReportTimeType == parameter.ReportTimeType
            //         && x.ProductReportType == parameter.ProductReportType
            //         && x.ProductDetailId == parameter.ProductDetailId
            //         && x.Date < date
            //         && x.ShopId == parameter.ReportShopId).OrderByDescending(x => x.Date).FirstOrDefault();
            //return report != null ? report.EndingQuantity : 0;
            return 0;
        }

        public ProductReportService(Repo repository)
            : base(repository)
        {
        }
    }
}
