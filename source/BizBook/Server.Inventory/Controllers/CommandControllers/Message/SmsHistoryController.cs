using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Message;
using RequestModel.Message;
using ViewModel.Message;

namespace Server.Inventory.Controllers.CommandControllers.Message
{
    [RoutePrefix("api/SmsHistory")]
    public class SmsHistoryController : BaseCommandController<SmsHistory, SmsHistoryRequestModel, SmsHistoryViewModel>
    {
        public SmsHistoryController() : base(new BaseService<SmsHistory, SmsHistoryRequestModel, SmsHistoryViewModel>(new BaseRepository<SmsHistory>(BusinessDbContext.Create())))
        {

        }       
    }
}
