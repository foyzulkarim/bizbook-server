using Model.Message;
using RequestModel.Message;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Service;
using Model;
using CommonLibrary.Repository;

namespace Server.Inventory.Controllers.QueryControllers.Message
{
    [RoutePrefix("api/SmsQuery")]
    public class SmsQueryController : BaseQueryController<Sms, MessageRequestModel, SmsViewModel>
    {
        public SmsQueryController() : base(new BaseService<Sms, MessageRequestModel, SmsViewModel>(new BaseRepository<Sms>(BusinessDbContext.Create())))
        {

        }
    }
}
