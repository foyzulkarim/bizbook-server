namespace ViewModel.Warehouses
{
    using CommonLibrary.ViewModel;
    using Model.Warehouses;

    public class WarehouseViewModel : BaseViewModel<Warehouse>
    {
        public WarehouseViewModel(Warehouse entity) : base(entity)
        {
            Name = entity.Name;
            StreetAddress = entity.StreetAddress;
            Area = entity.Area;
            District = entity.District;
            IsMain = entity.IsMain;
            ShopId = entity.ShopId;
        }

        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string Area { get; set; }

        public string District { get; set; }

        public bool IsMain { get; set; }
        public string ShopId { get; set; }
    }
}