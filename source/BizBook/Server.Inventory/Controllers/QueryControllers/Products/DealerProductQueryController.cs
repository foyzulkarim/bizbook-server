using Model.Products;
using RequestModel.Products;
using System.Web.Http;
using ViewModel.Products;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.QueryControllers.Products
{
    [RoutePrefix("api/DealerProductQuery")]
    public class DealerProductQueryController : BaseQueryController<DealerProduct, DealerProductRequestModel, DealerProductViewModel>
    {
        public DealerProductQueryController() : base(new BaseService<DealerProduct, DealerProductRequestModel, DealerProductViewModel>(new BaseRepository<DealerProduct>(BusinessDbContext.Create())))
        {

        }
    }
}
