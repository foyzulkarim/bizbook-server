using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLibrary.Reports
{
    using System.Data.Entity;
    using System.Threading.Tasks;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Sales;

    using ReportModel;
    using ReportModel.Parameters;

    using RequestModel.Reports;
    using ViewModel.Reports;

    public class SaleReportService : BaseReportService
    {
        public SaleReportService()
        {
        }

        public bool QuickUpdate(DateTime date, string shopId)
        {
            BusinessDbContext db = BusinessDbContext.Create();
            ReportDbContext reportDb = new ReportDbContext();
            date = date.Date;

            var salesToday = db.Sales.Where(x => x.ShopId == shopId && DbFunctions.TruncateTime(x.Created) == date)
                                 .ToList();

            var report = reportDb.SaleReports.Where(x => x.ShopId == shopId).FirstOrDefault(x => x.Date == date);
            if (report == null)
            {
                bool b = ShopStart(date, shopId);
            }

            List<SaleReport> allReports = reportDb.SaleReports.Where(x => x.ShopId == shopId && DbFunctions.TruncateTime(x.Date) == date).ToList();
            SaleReport reportSaleTypeAll = allReports.First(x => x.SaleType == SaleType.All);
            CalculateAmounts(reportSaleTypeAll, salesToday);
            reportSaleTypeAll.Modified = DateTime.Now;
            reportDb.Entry(reportSaleTypeAll).State = EntityState.Modified;
            SaleReport reportDealerSale = allReports.First(x => x.SaleType == SaleType.DealerSale);
            List<Sale> dealerSalesToday = salesToday.Where(x => x.IsDealerSale).ToList();
            CalculateAmounts(reportDealerSale, dealerSalesToday);
            reportDealerSale.Modified = DateTime.Now;
            reportDb.Entry(reportDealerSale).State = EntityState.Modified;
            SaleReport reportCustomerSale = allReports.First(x => x.SaleType == SaleType.CustomerSale);
            List<Sale> customerSalesToday = salesToday.Where(x => x.IsDealerSale == false).ToList();
            CalculateAmounts(reportCustomerSale, customerSalesToday);
            reportCustomerSale.Modified = DateTime.Now;
            reportDb.Entry(reportCustomerSale).State = EntityState.Modified;
            reportDb.SaveChanges();
            return true;
        }

        private void CalculateAmounts(SaleReport report, List<Sale> salesToday)
        {
            report.AmountProduct = salesToday.Sum(x => x.ProductAmount);
            report.AmountDeliveryCharge = salesToday.Sum(x => x.DeliveryChargeAmount);
            report.AmountTax = salesToday.Sum(x => x.TaxAmount);
            report.AmountOther = salesToday.Sum(x => x.OtherAmount);
            report.AmountTotal = salesToday.Sum(x => x.TotalAmount);
            report.AmountDiscount = salesToday.Sum(x => x.DiscountAmount);
            report.AmountPayable = salesToday.Sum(x => x.PayableTotalAmount);
            report.AmountPaid = salesToday.Sum(x => x.PaidAmount);
            report.AmountDue = salesToday.Sum(x => x.DueAmount);
            report.AmountCost = salesToday.Sum(x => x.CostAmount);
            report.CountSaleCreated = salesToday.Count;
            report.CountSaleCompleted = salesToday.Count(x => x.OrderState == OrderState.Completed);
        }

        public bool ShopStart(DateTime date, string shopId)
        {
            ReportDbContext reportDb = ReportDbContext.Create();
            date = date.Date;

            SaleReport report = reportDb.SaleReports.Where(x => x.ShopId == shopId).FirstOrDefault(x => x.Date == date);
            if (report == null)
            {
                report = new SaleReport { SaleType = SaleType.All };
                this.SetDefaults(report, shopId, date);

                var dealerReport = new SaleReport { SaleType = SaleType.DealerSale };
                this.SetDefaults(dealerReport, shopId, date);

                var customerReport = new SaleReport { SaleType = SaleType.CustomerSale };
                this.SetDefaults(customerReport, shopId, date);

                var collection = new List<SaleReport>() { report, dealerReport, customerReport };
                reportDb.SaleReports.AddRange(collection);
                int i = reportDb.SaveChanges();
                return i > 0;
            }

            return true;
        }

        public async Task<Tuple<List<SaleReportViewModel>, int>> SearchAsync(SaleReportRequestModel request)
        {
            ReportDbContext db = new ReportDbContext();
            BaseRepository<SaleReport> repo = new BaseRepository<SaleReport>(db);
            var service = new BaseService<SaleReport, SaleReportRequestModel, SaleReportViewModel>(repo);
            var tuple = await service.SearchAsync(request);
            return tuple;
        }
    }
}