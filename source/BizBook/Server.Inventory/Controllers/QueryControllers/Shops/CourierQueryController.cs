using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Shops;
using RequestModel.Shops;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.QueryControllers.Shops
{
    [RoutePrefix("api/CourierQuery")]
    public class CourierQueryController : BaseQueryController<Courier,CourierRequestModel,CourierViewModel>
    {
        public CourierQueryController() : base(new BaseService<Courier, CourierRequestModel, CourierViewModel>(new BaseRepository<Courier>(BusinessDbContext.Create())))
        {
        }
    }
}
