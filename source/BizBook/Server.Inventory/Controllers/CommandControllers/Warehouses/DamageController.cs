using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Warehouses;
using RequestModel.Warehouses;
using ViewModel.Warehouses;

namespace Server.Inventory.Controllers.CommandControllers.Warehouses
{
    [RoutePrefix("api/Damage")]
    public class DamageController : BaseCommandController<Damage,DamageRequestModel,DamageViewModel>
    {
        public DamageController() : base(new BaseService<Damage, DamageRequestModel, DamageViewModel>(new BaseRepository<Damage>(BusinessDbContext.Create())))
        {
        }
    }
}
