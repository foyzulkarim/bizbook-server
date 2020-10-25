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
            BusinessSeedData.AddSysShop(context);
            const string demoShopId = "00000000-0000-0000-0000-000000000001";
            BusinessSeedData.AddShop(context, demoShopId, "Demo1");
            BusinessSeedData.AddBrand(demoShopId, context);
            BusinessSeedData.AddAccountHeads(context, demoShopId);
            BusinessSeedData.AddWallet(context, demoShopId);
            BusinessSeedData.AddProducts(context, demoShopId);
            BusinessSeedData.AddSupplier(context, demoShopId);
            BusinessSeedData.AddCustomer(context, demoShopId);
            BusinessSeedData.AddWarehouse(context, demoShopId);            
        }
    }
}