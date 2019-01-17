using Model.Message;
using RequestModel.Message;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.QueryControllers.Message
{
    [RoutePrefix("api/HookDetailQuery")]
    public class HookDetailQueryController : BaseQueryController<HookDetail, HookDetailRequestModel, HookDetailViewModel>
    {
        public HookDetailQueryController() : base(new BaseService<HookDetail, HookDetailRequestModel, HookDetailViewModel>(new BaseRepository<HookDetail>(BusinessDbContext.Create())))
        {
        }
    }
}
