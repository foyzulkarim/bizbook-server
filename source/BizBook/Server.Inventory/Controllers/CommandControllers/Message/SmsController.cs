using Model.Message;
using RequestModel.Message;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;

namespace Server.Inventory.Controllers.CommandControllers.Message
{
    [RoutePrefix("api/Sms")]
    public class SmsController : BaseCommandController<Sms, SmsRequestModel, SmsViewModel>
    {
        public SmsController() : base(new BaseService<Sms, SmsRequestModel, SmsViewModel>(new BaseRepository<Sms>(BusinessDbContext.Create())))
        {
        }
    }
}
