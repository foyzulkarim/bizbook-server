
using Vm = ViewModel.Sales.SaleDetailViewModel;
using Rm = RequestModel.Sales.SaleDetailRequestModel;
using Repo = CommonLibrary.Repository.BaseRepository<Model.Sales.SaleDetail>;
using M = Model.Sales.SaleDetail;
using CommonLibrary.Service;

namespace ServiceLibrary.Sales
{
    public class SaleDetailService : BaseService<M, Rm, Vm>
    {
        public SaleDetailService(Repo repository) : base(repository)
        {
            //customerRepo = new BaseRepository<Customer>(base.Repository.Db);
            //customerService = new CustomerService(customerRepo);
        }
    }
}
