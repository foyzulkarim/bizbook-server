using Model.Employees;
using RequestModel.Employees;
using System.Web.Http;
using ViewModel.Employees;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.CommandControllers.Employees
{
    [RoutePrefix("api/EmployeeInfo")]
    public class EmployeeInfoController : BaseCommandController<EmployeeInfo, EmployeeInfoRequestModel, EmployeeInfoViewModel>
    {
        public EmployeeInfoController() : base(new BaseService<EmployeeInfo, EmployeeInfoRequestModel, EmployeeInfoViewModel>(new BaseRepository<EmployeeInfo>(BusinessDbContext.Create())))
        {

        }
    }
}
