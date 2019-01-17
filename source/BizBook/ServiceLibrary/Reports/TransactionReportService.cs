using System;
using System.Collections.Generic;
using System.Linq;

using Model;
using T = Model.Transactions.Transaction;
using Vm = ViewModel.Reports.TransactionReportViewModel;
using M = Model.Reports.TransactionReport;
using Rm = RequestModel.Reports.TransactionReportRequestModel;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Reports.TransactionReport>;

namespace ServiceLibrary.Reports
{
    using ReportModel.Parameters;

    public class TransactionReportService : BaseReportService<M, Rm, Vm, T>
    {
        public TransactionReportService(Repo repository)
            : base(repository)
        {
            
        }
        
        private Dictionary<TransactionMedium, double> GetPreviousEnding(TransactionReportParameter parameter)
        {
            Dictionary<TransactionMedium, double> previousEnding = new Dictionary<TransactionMedium, double>();

            var report = this.db.TransactionReports.Where(
                x => x.ReportTimeType == parameter.ReportTimeType
                     && x.TransactionReportType == parameter.TransactionReportType
                     && x.AccountHeadId == parameter.AccountHeadId && x.Date < parameter.Date
                     && x.ShopId == parameter.ReportShopId).OrderByDescending(x => x.Date).FirstOrDefault();

            if (report != null)
            {
                previousEnding.Add(TransactionMedium.All, report.TotalEnding);
                previousEnding.Add(TransactionMedium.Cash, report.CashEnding);
                previousEnding.Add(TransactionMedium.Card, report.CardEnding);
                previousEnding.Add(TransactionMedium.Cheque, report.ChequeEnding);
                previousEnding.Add(TransactionMedium.Mobile, report.MobileEnding);
                previousEnding.Add(TransactionMedium.Other, report.OtherEnding);
            }
            else
            {
                previousEnding.Add(TransactionMedium.All, 0);
                previousEnding.Add(TransactionMedium.Cash, 0);
                previousEnding.Add(TransactionMedium.Card, 0);
                previousEnding.Add(TransactionMedium.Cheque, 0);
                previousEnding.Add(TransactionMedium.Mobile, 0);
                previousEnding.Add(TransactionMedium.Other, 0);
            }

            return previousEnding;
        }

        public override IEnumerable<M> CreateReportModels(List<T> models, ReportParameterBase p)
        {
            string defaultAccountHeadId = new Guid().ToString();
            string defaultAccountHeadName = "All";
            TransactionReportParameter previousParameter = new TransactionReportParameter(
                TransactionReportType.TransactionByAmount,
                defaultAccountHeadName,
                defaultAccountHeadId,
                null,
                p);

            var startingAll = this.GetPreviousEnding(previousParameter);

            var parameterAll = new TransactionReportParameter(
                TransactionReportType.TransactionByAmount,
                defaultAccountHeadName,
                defaultAccountHeadId,
                startingAll,
                p);

            var allReport = this.CreateReportModel(models, parameterAll);
            yield return allReport;

            List<IGrouping<string, T>> transactionsByAccId = models.GroupBy(x => x.AccountHeadId).ToList();
            foreach (var tById in transactionsByAccId)
            {
                var accId = tById.Key;
                if (tById.Any())
                {
                    var transaction = tById.First();
                    previousParameter.TransactionReportType = TransactionReportType.TransactionByAccountHead;
                    previousParameter.AccountHeadId = accId;
                    Dictionary<TransactionMedium, double> startingAmountsByAccount = GetPreviousEnding(previousParameter);
                    var parameterByAccount = new TransactionReportParameter(TransactionReportType.TransactionByAccountHead, transaction.AccountHeadName, accId, startingAmountsByAccount, p);
                    M transactionReport = this.CreateReportModel(tById.ToList(), parameterByAccount);
                    yield return transactionReport;
                }
            }
        }

        public override M CreateReportModel(List<T> models, ReportParameterBase reportParameter)
        {
            var parameter = (TransactionReportParameter)reportParameter;
            M report = new M();
            report.Id = Guid.NewGuid().ToString();
            report.CreatedFrom = "BizBook";
            report.CreatedBy = "Automatic";
            report.ModifiedBy = "Automatic";
            report.Created = DateTime.Today.Date;
            report.Modified = DateTime.Today.Date;
            report.ShopId = reportParameter.ReportShopId;

            report.ReportTimeType = reportParameter.ReportTimeType;
            report.Date = reportParameter.Date.Date;
            report.Value = reportParameter.Value;
            report.RowsCount = models.Count;

            report.TransactionReportType = parameter.TransactionReportType;
            report.AccountHeadId = parameter.AccountHeadId;
            report.AccountHeadName = parameter.AccountHeadName;

            report.TotalStarting = parameter.TotalStarting;
            report.CashStarting = parameter.CashStarting;
            report.CardStarting = parameter.CardStarting;
            report.ChequeStarting = parameter.ChequeStarting;
            report.MobileStarting = parameter.MobileStarting;
            report.OtherStarting = parameter.OtherStarting;

            report.TotalRowsCount = models.Count;
            report.TotalIn = models.GetIncome();
            report.TotalOut = models.GetExpense();
            report.TotalEnding = report.TotalStarting + report.TotalIn - report.TotalOut;

            var cashTransactions = models.Where(x => x.TransactionMedium == TransactionMedium.Cash).ToList();
            report.CashRowsCount = cashTransactions.Count;
            report.CashIn = cashTransactions.GetIncome();
            report.CashOut = cashTransactions.GetExpense();
            report.CashEnding = report.CashStarting + report.CashIn - report.CashOut;

            var cardTransactions = models.Where(x => x.TransactionMedium == TransactionMedium.Card).ToList();
            report.CardRowsCount = cardTransactions.Count;
            report.CardIn = cardTransactions.GetIncome();
            report.CardOut = cardTransactions.GetExpense();
            report.CardEnding = report.CardStarting + report.CardIn - report.CardOut;

            var chequeTransactions = models.Where(x => x.TransactionMedium == TransactionMedium.Cheque).ToList();
            report.ChequeRowsCount = chequeTransactions.Count;
            report.ChequeIn = chequeTransactions.GetIncome();
            report.ChequeOut = chequeTransactions.GetExpense();
            report.ChequeEnding = report.ChequeStarting + report.ChequeIn - report.ChequeOut;

            var mobileTransactions = models.Where(x => x.TransactionMedium == TransactionMedium.Mobile).ToList();
            report.MobileRowsCount = mobileTransactions.Count;
            report.MobileIn = mobileTransactions.GetIncome();
            report.MobileOut = mobileTransactions.GetExpense();
            report.MobileEnding = report.MobileStarting + report.MobileIn - report.MobileOut;

            var otherTransactions = models.Where(x => x.TransactionMedium == TransactionMedium.Other).ToList();
            report.OtherRowsCount = otherTransactions.Count;
            report.OtherIn = otherTransactions.GetIncome();
            report.OtherOut = otherTransactions.GetExpense();
            report.OtherEnding = report.OtherStarting + report.OtherIn - report.OtherOut;

            return report;
        }

        public override void SaveReports(List<M> reports)
        {
            foreach (M report in reports)
            {
                M dbReport = this.db.TransactionReports.FirstOrDefault(
                    x => x.ShopId == report.ShopId 
                        && x.ReportTimeType == report.ReportTimeType
                        && x.Date == report.Date
                        && x.TransactionReportType == report.TransactionReportType
                        && x.AccountHeadId == report.AccountHeadId);
                if (dbReport == null)
                {
                    this.db.TransactionReports.Add(report);                 
                }
                else
                {
                    dbReport.TotalStarting = report.TotalStarting;
                    dbReport.CashStarting = report.CashStarting;
                    dbReport.CardStarting = report.CardStarting;
                    dbReport.ChequeStarting = report.ChequeStarting;
                    dbReport.MobileStarting = report.MobileStarting;
                    dbReport.OtherStarting = report.OtherStarting;

                    dbReport.TotalRowsCount = report.TotalRowsCount;
                    dbReport.TotalIn = report.TotalIn;
                    dbReport.TotalOut = report.TotalOut;
                    dbReport.TotalEnding = report.TotalEnding;

                    dbReport.CashRowsCount = report.CashRowsCount;
                    dbReport.CashIn = report.CashIn;
                    dbReport.CashOut = report.CashOut;
                    dbReport.CashEnding = report.CashEnding;

                    dbReport.CardRowsCount = report.CardRowsCount;
                    dbReport.CardIn = report.CardIn;
                    dbReport.CardOut = report.CardOut;
                    dbReport.CardEnding = report.CardEnding;

                    dbReport.ChequeRowsCount = report.ChequeRowsCount;
                    dbReport.ChequeIn = report.ChequeIn;
                    dbReport.ChequeOut = report.ChequeOut;
                    dbReport.ChequeEnding = report.ChequeEnding;

                    dbReport.MobileRowsCount = report.MobileRowsCount;
                    dbReport.MobileIn = report.MobileIn;
                    dbReport.MobileOut = report.MobileOut;
                    dbReport.MobileEnding = report.MobileEnding;


                    dbReport.OtherRowsCount = report.OtherRowsCount;
                    dbReport.OtherIn = report.OtherIn;
                    dbReport.OtherOut = report.OtherOut;
                    dbReport.OtherEnding = report.OtherEnding;
                }

                this.db.SaveChanges();
            }
        }
    }

    public static class Extensions
    {
        public static double GetIncome(this List<T> models)
        {
            double sum = models.Where(x => x.TransactionFlowType == TransactionFlowType.Income).Sum(x => x.Amount);
            return sum;
        }

        public static double GetExpense(this List<T> models)
        {
            double sum = models.Where(x => x.TransactionFlowType == TransactionFlowType.Expense).Sum(x => x.Amount);
            return sum;
        }
    }

}
