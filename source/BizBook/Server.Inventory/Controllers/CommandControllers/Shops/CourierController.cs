using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Shops;
using RequestModel.Shops;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.CommandControllers.Shops
{
    [RoutePrefix("api/Courier")]
    public class CourierController : BaseCommandController<Courier,CourierRequestModel,CourierViewModel>
    {
        public CourierController() : base(new BaseService<Courier, CourierRequestModel, CourierViewModel>(new BaseRepository<Courier>(BusinessDbContext.Create())))
        {
        }
    }
}
