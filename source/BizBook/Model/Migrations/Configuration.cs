namespace Model.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BusinessDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BusinessDbContext context)
        {
            //const string shopId = "00000000-0000-0000-0000-000000000001";
            //BusinessSeedData.AddSysShop(context);
            //BusinessSeedData.AddShop(context, shopId, "Demo1");
            //BusinessSeedData.AddBrand(shopId, context);
            //BusinessSeedData.AddAccountHeads(context, shopId);
            //BusinessSeedData.AddAccountInfo(context, shopId);
            //BusinessSeedData.AddProducts(context, shopId);
            //BusinessSeedData.AddSupplier(context, shopId);
            //BusinessSeedData.AddCustomer(context, shopId);
            //BusinessSeedData.AddWarehouses(context); // manually sql query to update the whid in tables
            //BusinessSeedData.UpdateWarehouseRelatedData(context);
            //BusinessSeedData.UpdateSaleDetailType(context);
        }
    }
}