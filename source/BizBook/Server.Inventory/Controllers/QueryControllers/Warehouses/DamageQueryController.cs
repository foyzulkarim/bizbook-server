using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Warehouses;
using RequestModel.Warehouses;
using ViewModel.Warehouses;

namespace Server.Inventory.Controllers.QueryControllers.Warehouses
{
    [RoutePrefix("api/DamageQuery")]
    public class DamageQueryController : BaseQueryController<Damage,DamageRequestModel,DamageViewModel>
    {
        public DamageQueryController() : base(new BaseService<Damage, DamageRequestModel, DamageViewModel>(new BaseRepository<Damage>(BusinessDbContext.Create())))
        {
        }
    }
}
