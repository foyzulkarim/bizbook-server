using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model.Purchases;
using Model.Sales;
using Model.Warehouses;
using RequestModel.Purchases;
using RequestModel.Sales;
using RequestModel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.History;
using ViewModel.Purchases;
using ViewModel.Sales;
using ViewModel.Warehouses;

namespace ServiceLibrary.Warehouses
{
    public class WarehouseService : BaseService<Warehouse, WarehouseRequestModel, WarehouseViewModel>
    {
        public WarehouseService(BaseRepository<Warehouse> repository) : base(repository)
        {
        }

        public async Task<Tuple<WarehouseViewModel, List<HistoryViewModel>, int>> GetHistory(WarehouseRequestModel request)
        {
            var wareService = new BaseService<Warehouse, WarehouseRequestModel, WarehouseViewModel>(new BaseRepository<Warehouse>(Repository.Db));

            var warehouse = wareService.GetById(request.ParentId);

            var warehouseViewModel = new WarehouseViewModel(warehouse);

            var saleService = new BaseService<SaleDetail, SaleDetailRequestModel, SaleDetailViewModel>(new BaseRepository<SaleDetail>(Repository.Db));

            var saleDetailRequestModel = new SaleDetailRequestModel("")
            {
                ShopId = request.ShopId,
                WarehouseId = request.ParentId,
                Page = -1,
                IsIncludeParents = true
            };

            Tuple<List<SaleDetailViewModel>, int> result = await saleService.SearchAsync(saleDetailRequestModel);

            List<HistoryViewModel> viewModels = result.Item1.ConvertAll(x => new HistoryViewModel(x, x.ProductDetailName, x.SalePricePerUnit)).ToList();

            var purchaseDetailService = new BaseService<PurchaseDetail, PurchaseDetailRequestModel, PurchaseDetailViewModel>(new BaseRepository<PurchaseDetail>(Repository.Db));
            var purchaseDetailRequestModel = new PurchaseDetailRequestModel("")
            {
                ShopId = request.ShopId,
                WarehouseId = request.ParentId,
                //ProductDetailId = request.ParentId,
                Page = -1,
                IsIncludeParents = true,
            };

            Tuple<List<PurchaseDetailViewModel>, int> purchaseDetailResult = await purchaseDetailService.SearchAsync(purchaseDetailRequestModel);
            List<HistoryViewModel> models = purchaseDetailResult.Item1.ConvertAll(x => new HistoryViewModel(x, x.ProductDetailName, x.CostPricePerUnit)).ToList();
            viewModels.AddRange(models);
            List<HistoryViewModel> merged = viewModels.OrderByDescending(x => x.Date).ToList();

            return new Tuple<WarehouseViewModel, List<HistoryViewModel>, int>(warehouseViewModel, viewModels, viewModels.Count);

        }
    }
}
