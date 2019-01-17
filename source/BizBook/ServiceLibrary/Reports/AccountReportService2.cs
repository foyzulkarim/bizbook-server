namespace ServiceLibrary.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;

    using ReportModel;

    using RequestModel.Reports;

    using ViewModel.Reports;

    public class AccountReportService2 : BaseReportService, IReportService2
    {
        public string QuickUpdate(string shopId, string itemId, DateTime date)
        {
            date = date.Date;
            ReportDbContext rDb = new ReportDbContext();
            BusinessDbContext bDb = new BusinessDbContext();

            AccountReport report = rDb.AccountReports.FirstOrDefault(
                x => x.ShopId == shopId && x.AccountHeadId == itemId && DbFunctions.TruncateTime(x.Date) == date);
            if (report == null)
            {
                report = this.GetReportObject(itemId, bDb);
                this.SetDefaults(report, shopId, date);
                rDb.AccountReports.Add(report);
                rDb.SaveChanges();
                report = rDb.AccountReports.First(x => x.Id == report.Id);
            }

            var incomeDetails = bDb.Transactions
                .Where(x => x.ShopId == shopId && DbFunctions.TruncateTime(x.Created) == date).Where(
                    x => x.AccountHeadId == itemId && x.TransactionFlowType == TransactionFlowType.Income)
                .AsQueryable();

            var expenseDetails = bDb.Transactions
                .Where(x => x.ShopId == shopId && DbFunctions.TruncateTime(x.Created) == date).Where(
                    x => x.AccountHeadId == itemId && x.TransactionFlowType == TransactionFlowType.Expense)
                .AsQueryable();

            var incomesToday = incomeDetails.ToList();
            var expensesToday = expenseDetails.ToList();

            report.AmountTotalIn = incomesToday.Sum(x => x.Amount);
            report.AmountTotalOut = expensesToday.Sum(x => x.Amount);
            report.AmountTotalEnding = report.AmountTotalStarting + report.AmountTotalIn - report.AmountTotalOut;
            report.CountTotalTrx = incomesToday.Count + expensesToday.Count;

            // cash
            var cashIns = incomesToday.Where(x => x.TransactionMedium == TransactionMedium.Cash).ToList();
            report.AmountCashIn = cashIns.Sum(x => x.Amount);

            var cashOuts = expensesToday.Where(x => x.TransactionMedium == TransactionMedium.Cash).ToList();
            report.AmountCashOut = cashOuts.Sum(x => x.Amount);

            report.AmountCashEnding = report.AmountCashStarting + report.AmountCashIn - report.AmountCashOut;
            report.CountCashTrx = cashIns.Count + cashOuts.Count;

            // bank
            var bankIns = incomesToday.Where(
                    x => x.TransactionMedium == TransactionMedium.Bank || x.TransactionMedium == TransactionMedium.Card
                                                                       || x.TransactionMedium
                                                                       == TransactionMedium.Cheque)
                .ToList();
            report.AmountBankIn = bankIns.Sum(x => x.Amount);

            var bankOuts = expensesToday.Where(
                    x => x.TransactionMedium == TransactionMedium.Bank || x.TransactionMedium == TransactionMedium.Card
                                                                       || x.TransactionMedium
                                                                       == TransactionMedium.Cheque)
                .ToList();
            report.AmountBankOut = bankOuts.Sum(x => x.Amount);

            report.AmountBankEnding = report.AmountBankStarting + report.AmountBankIn - report.AmountBankOut;
            report.CountBankTrx = bankIns.Count + bankOuts.Count;

            // mobile
            var mobileIns = incomesToday.Where(x => x.TransactionMedium == TransactionMedium.Mobile).ToList();
            report.AmountMobileIn = mobileIns.Sum(x => x.Amount);

            var mobileOuts = expensesToday.Where(x => x.TransactionMedium == TransactionMedium.Mobile).ToList();
            report.AmountMobileOut = mobileOuts.Sum(x => x.Amount);

            report.AmountMobileEnding = report.AmountMobileStarting + report.AmountMobileIn - report.AmountMobileOut;
            report.CountMobileTrx = mobileIns.Count + mobileOuts.Count;

            // other
            var otherIns = incomesToday.Where(x => x.TransactionMedium == TransactionMedium.Other).ToList();
            report.AmountOtherIn = otherIns.Sum(x => x.Amount);

            var otherOuts = expensesToday.Where(x => x.TransactionMedium == TransactionMedium.Other).ToList();
            report.AmountOtherOut = otherOuts.Sum(x => x.Amount);

            report.AmountOtherEnding = report.AmountOtherStarting + report.AmountOtherIn - report.AmountOtherOut;
            report.CountOtherTrx = otherIns.Count + otherOuts.Count;

            report.Modified = DateTime.Now;
            int i = rDb.SaveChanges();
            return report.Id;
        }

        public bool DayStartAll(string shopId, DateTime date)
        {
            date = date.Date;
            DateTime yesterday = date.AddDays(-1).Date;

            ReportDbContext rDb = new ReportDbContext();
            BusinessDbContext bDb = new BusinessDbContext();
            List<string> ids = bDb.AccountHeads.Where(x => x.ShopId == shopId).Select(x => x.Id).ToList();

            foreach (var id in ids)
            {
                this.QuickUpdate(shopId, id, yesterday);
            }

            foreach (string id in ids)
            {
                var todayReport = this.GetReportObject(id, bDb);
                this.SetDefaults(todayReport, shopId, date);

                var yesterdayReport = rDb.AccountReports.Where(x => x.ShopId == shopId && x.AccountHeadId == id)
                    .FirstOrDefault(x => DbFunctions.TruncateTime(x.Date) == yesterday);
                if (yesterdayReport != null)
                {
                    todayReport.AmountTotalStarting = yesterdayReport.AmountTotalEnding;
                }

                var todayExists = rDb.AccountReports.Any(
                    x => x.ShopId == shopId && x.AccountHeadId == id && DbFunctions.TruncateTime(x.Date) == date);
                if (todayExists)
                {
                    this.QuickUpdate(shopId, id, date);
                }
                else
                {
                    rDb.AccountReports.Add(todayReport);
                    rDb.SaveChanges();
                }
            }

            return true;
        }

        private AccountReport GetReportObject(string itemId, BusinessDbContext bDb)
        {
            var detail = bDb.AccountHeads.First(x => x.Id == itemId);
            var report = new AccountReport { AccountHeadId = detail.Id, AccountHeadName = detail.Name };
            return report;
        }

        public async Task<Tuple<List<AccountReportViewModel>, int>> SearchAsync(AccountReportRequestModel request)
        {
            ReportDbContext db = new ReportDbContext();
            BaseRepository<AccountReport> repo = new BaseRepository<AccountReport>(db);
            var service = new BaseService<AccountReport, AccountReportRequestModel, AccountReportViewModel>(repo);
            var tuple = await service.SearchAsync(request);
            return tuple;
        }
    }
}