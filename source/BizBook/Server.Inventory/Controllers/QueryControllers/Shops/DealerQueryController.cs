using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using RequestModel.Shops;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.QueryControllers.Shops
{
    using Model.Dealers;

    [RoutePrefix("api/DealerQuery")]
    public class DealerQueryController : BaseQueryController<Dealer, DealerRequestModel, DealerViewModel>
    {
        public DealerQueryController() : base(new BaseService<Dealer, DealerRequestModel, DealerViewModel>(new BaseRepository<Dealer>(BusinessDbContext.Create())))
        {
        }
    }
}
