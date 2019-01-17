using System;

namespace ServiceLibrary.Reports
{
    using ReportModel;

    public class BaseReportService
    {
        protected T SetDefaults<T>(T report, string shopId, DateTime date) where T : BaseReport
        {
            report.Id = Guid.NewGuid().ToString();
            DateTime dateTime = DateTime.Now;
            report.Created = dateTime;
            report.Modified = dateTime;
            report.CreatedBy = "System";
            report.ModifiedBy = "System";
            report.CreatedFrom = "System";
            report.IsActive = true;
            
            report.Date = date;
            report.Value = date.ToString("dd/MM/yyyy");
            report.RowsCount = 0;
            report.ShopId = shopId;
            return report;
        }
    }
}
