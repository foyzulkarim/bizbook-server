using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Shops;
using RequestModel.Shops;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.CommandControllers.Shops
{
    [RoutePrefix("api/Brand")]
    public class BrandController : BaseCommandController<Brand, BrandRequestModel, BrandViewModel>
    {
        public BrandController(): base(new BaseService<Brand, BrandRequestModel, BrandViewModel>(new BaseRepository<Brand>(BusinessDbContext.Create())))
        {
            
        }
    }
}
