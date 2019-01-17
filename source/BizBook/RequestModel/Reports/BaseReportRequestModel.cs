namespace RequestModel.Reports
{
    using CommonLibrary.Model;
    using CommonLibrary.RequestModel;
    using Model;

    public abstract class BaseReportRequestModel<T> : RequestModel<T> where T : Entity
    {
        public ReportTimeType ReportTimeType { get; set; }

        protected BaseReportRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }
    }
}