using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Message;
using RequestModel.Message;
using ViewModel.Message;

namespace Server.Inventory.Controllers.QueryControllers.Message
{
    [RoutePrefix("api/SmsHistoryQuery")]
    public class SmsHistoryQueryController : BaseQueryController<SmsHistory, SmsHistoryRequestModel,SmsHistoryViewModel>
    {
        public SmsHistoryQueryController() : base(new BaseService<SmsHistory, SmsHistoryRequestModel, SmsHistoryViewModel>(new BaseRepository<SmsHistory>(BusinessDbContext.Create())))
        {
        }
    }
}
