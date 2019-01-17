using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using RequestModel.Shops;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.CommandControllers.Shops
{
    using Model.Dealers;

    [RoutePrefix("api/Dealer")]
    public class DealerController : BaseCommandController<Dealer,DealerRequestModel,DealerViewModel>
    {
        public DealerController() : base(new BaseService<Dealer, DealerRequestModel, DealerViewModel>(new BaseRepository<Dealer>(BusinessDbContext.Create())))
        {
        }
    }
}
