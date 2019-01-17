using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Rm = RequestModel.Customers.CustomerFeedbackRequestModel;
using M = Model.Customers.CustomerFeedback;
using Vm = ViewModel.Customers.CustomerFeedbackViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Customers
{
    [RoutePrefix("api/CustomerFeedbackQuery")]
    public class CustomerFeedbackQueryController : BaseQueryController<M, Rm, Vm>
    {
        public CustomerFeedbackQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }
    }
}
