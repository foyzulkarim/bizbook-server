using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model.Shops;
using RequestModel.Shops;
using ViewModel.Shops;

namespace ServiceLibrary.Shops
{
    public class ShopService : BaseService<Shop, ShopRequestModel, ShopSuperAdminViewModel>
    {
        public ShopService(BaseRepository<Shop> repository) : base(repository)
        {
        }

        public virtual ShopViewModel GetMyDetail(string id)
        {
            var model = Repository.GetById(id);
            if (model == null)
            {
                return null;
            }
            var myDetail = new ShopViewModel(model);
            return myDetail;
        }
    }
}