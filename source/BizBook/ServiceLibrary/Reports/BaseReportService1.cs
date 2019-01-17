using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLibrary.Reports
{
    using System.Data.Entity.SqlServer;
    using System.Globalization;
    using System.Linq.Expressions;
    using CommonLibrary.Repository;
    using CommonLibrary.RequestModel;
    using CommonLibrary.Service;
    using CommonLibrary.ViewModel;

    using Model;

    using ReportModel;
    using ReportModel.Parameters;

    public abstract class BaseReportService1<M, Rm, Vm, T> : BaseService<M, Rm, Vm>, IReportService<T, M>
        where M : BaseReport where Rm : RequestModel<M> where Vm : BaseViewModel<M> where T : ShopChild
    {
        protected BusinessDbContext db;

        protected Expression<Func<T, int?>> dayKeySelector = x => SqlFunctions.DatePart("dayofyear", x.Modified);
        protected Calendar calendar = new GregorianCalendar();
        protected Expression<Func<T, int?>> weekKeySelector = x => SqlFunctions.DatePart("week", x.Modified);
        protected Expression<Func<T, int?>> monthKeySelector = x => SqlFunctions.DatePart("month", x.Modified);
        protected Expression<Func<T, int?>> yearKeySelector = x => SqlFunctions.DatePart("year", x.Modified);


        protected BaseReportService1(BaseRepository<M> repository)
            : base(repository)
        {
            this.db = this.Repository.Db as BusinessDbContext;
        }

        public virtual void ShopClose(string shopId, DateTime start, DateTime end, ReportTimeType reportTimeType)
        {
            IQueryable<T> models = this.db.Set<T>().AsQueryable();
            Expression<Func<T, bool>> expression = x => x.ShopId == shopId && x.Modified >= start && x.Modified <= end;
            IQueryable<T> allModelsForTheShop = models.Where(expression).OrderByDescending(x => x.Modified).AsQueryable();
            switch (reportTimeType)
            {
                case ReportTimeType.Daily:
                    var years = allModelsForTheShop.GroupBy(this.yearKeySelector).Select(x=>x.Key).ToList();
                    foreach (var year in years)
                    {
                        List<IGrouping<int?, T>> dailyGroupedModels = allModelsForTheShop
                            .Where(x => x.Modified.Year == year).GroupBy(dayKeySelector).OrderBy(x => x.Key)
                            .ToList();

                        foreach (IGrouping<int?, T> dailyGrouped in dailyGroupedModels)
                        {
                            this.ProcessShopClose(shopId, dailyGrouped);
                        }

                        // Parallel.ForEach(dailyGroupedModels, x => ProcessShopClose(shopId, end, x));
                    }
                    
                    break;

                case ReportTimeType.Weekly:
                    var weeklySales = allModelsForTheShop.GroupBy(weekKeySelector).OrderBy(x => x.Key).ToList();
                    foreach (var week in weeklySales)
                    {
                        if (week.Any())
                        {
                            List<T> weeklySalesList = week.OrderBy(x => x.Modified).ToList();
                            DateTime date = weeklySalesList.First().Modified.Date;
                            string value = "Week " + calendar.GetWeekOfYear(
                                               date,
                                               CalendarWeekRule.FirstDay,
                                               DayOfWeek.Sunday) + "-" + date.Year;
                            ReportParameterBase p = new ReportParameterBase(date, value, shopId);
                            var reports = this.CreateReportModels(weeklySalesList, p).ToList();
                            this.SaveReports(reports);
                        }
                    }
                    break;
                case ReportTimeType.Monthly:
                    var monthlySales = allModelsForTheShop.GroupBy(monthKeySelector).OrderBy(x => x.Key).ToList();
                    foreach (var month in monthlySales)
                    {
                        DateTime date = new DateTime(end.Year, month.Key.Value, 1);
                        if (month.Any())
                        {
                            ReportParameterBase p = new ReportParameterBase(
                                date,
                                date.ToString("MMMM-yyyy"),
                                shopId);
                            var reports = this.CreateReportModels(month.ToList(), p).ToList();
                            this.SaveReports(reports);
                        }
                    }

                    break;
                case ReportTimeType.Yearly:
                    List<IGrouping<int?, T>> yearlySales = allModelsForTheShop.GroupBy(yearKeySelector).ToList();
                    foreach (var yearlySale in yearlySales)
                    {
                        DateTime date = new DateTime((int)yearlySale.Key, 1, 1);
                        ReportParameterBase p = new ReportParameterBase(date,
                            date.ToString("yyyy"), shopId);
                        var reports = this.CreateReportModels(yearlySale.ToList(), p).ToList();
                        this.SaveReports(reports);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reportTimeType), reportTimeType, null);
            }
        }

        private void ProcessShopClose(string shopId, IGrouping<int?, T> dailyGrouped)
        {
            if (dailyGrouped.Any())
            {
                var year = dailyGrouped.First().Modified.Year;
                DateTime date = new DateTime(year, 1, 1).AddDays(dailyGrouped.Key.Value - 1);
                Console.WriteLine($"Processing {date.ToString("dd-MMMM-yyyy")}");
                ReportParameterBase p = new ReportParameterBase(
                   
                    date,
                    date.ToString("dd-MMMM-yyyy"),
                    shopId);
                var reports = this.CreateReportModels(dailyGrouped.ToList(), p).ToList();
                this.SaveReports(reports);
            }
        }

        public abstract IEnumerable<M> CreateReportModels(List<T> models, ReportParameterBase p);

        public abstract M CreateReportModel(List<T> models, ReportParameterBase reportParameter);

        public abstract void SaveReports(List<M> reports);
    }
}
