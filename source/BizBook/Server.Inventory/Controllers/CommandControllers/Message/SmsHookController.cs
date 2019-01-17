using Model.Message;
using RequestModel.Message;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.CommandControllers.Message
{
    [RoutePrefix("api/SmsHook")]
    public class SmsHookController : BaseCommandController<SmsHook, SmsHookRequestModel, SmsHookViewModel>
    {
        public SmsHookController() : base(new BaseService<SmsHook, SmsHookRequestModel, SmsHookViewModel>(new BaseRepository<SmsHook>(BusinessDbContext.Create())))
        {
        }
    }
}
