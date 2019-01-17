using System.Web.Http;

namespace Server.Inventory.Controllers.QueryControllers.Sales
{
    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Sales;
    using RequestModel.Sales;
    using ViewModel.Sales;

    [RoutePrefix("api/InstallmentDetailQuery")]
    public class InstallmentDetailQueryController : BaseQueryController<InstallmentDetail, InstallmentDetailRequestModel, InstallmentDetailViewModel>
    {
        public InstallmentDetailQueryController() : base(new BaseService<InstallmentDetail, InstallmentDetailRequestModel, InstallmentDetailViewModel>(new BaseRepository<InstallmentDetail>(BusinessDbContext.Create())))
        {
        }

      
    }
}
