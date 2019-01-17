using Model.Employees;
using RequestModel.Employees;
using System.Web.Http;
using ViewModel.Employees;
using CommonLibrary.Service;
using Model;
using CommonLibrary.Repository;

namespace Server.Inventory.Controllers.QueryControllers.Employees
{
    [RoutePrefix("api/EmployeeInfoQuery")]
    public class EmployeeInfoQueryController : BaseQueryController<EmployeeInfo, EmployeeInfoRequestModel, EmployeeInfoViewModel>
    {
        public EmployeeInfoQueryController() : base(new BaseService<EmployeeInfo, EmployeeInfoRequestModel, EmployeeInfoViewModel>(new BaseRepository<EmployeeInfo>(BusinessDbContext.Create())))
        {

        }
    }
}
