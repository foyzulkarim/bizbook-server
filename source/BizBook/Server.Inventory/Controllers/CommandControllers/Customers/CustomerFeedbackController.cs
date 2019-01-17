using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using M = Model.Customers.CustomerFeedback;
using RM = RequestModel.Customers.CustomerFeedbackRequestModel;
using VM = ViewModel.Customers.CustomerFeedbackViewModel;

namespace Server.Inventory.Controllers.CommandControllers.Customers
{
    [RoutePrefix("api/CustomerFeedback")]
    public class CustomerFeedbackController : BaseCommandController<M,RM,VM>
    {
        public CustomerFeedbackController() : base(new BaseService<M, RM, VM>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }
    }
}