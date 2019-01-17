using Model.Message;
using RequestModel.Message;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Service;
using Model;
using CommonLibrary.Repository;

namespace Server.Inventory.Controllers.CommandControllers.Message
{
    [RoutePrefix("api/HookDetail")]
    public class HookDetailController : BaseCommandController<HookDetail, HookDetailRequestModel, HookDetailViewModel>
    {
        public HookDetailController() : base(new BaseService<HookDetail, HookDetailRequestModel, HookDetailViewModel>(new BaseRepository<HookDetail>(BusinessDbContext.Create())))
        {
        }
    }
}
