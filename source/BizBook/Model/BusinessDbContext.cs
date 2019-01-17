using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Model.Customers;
using Model.Products;
using Model.Purchases;
using Model.Sales;
using Model.Shops;
using Model.Warehouses;

namespace Model
{
    using Model.Message;
    using Model.Dealers;
    using Transactions;
    using Model.Employees;
    using Model.Operations;

    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext() : base("DefaultBusinessConnection")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public static BusinessDbContext Create()
        {
            return new BusinessDbContext();
        }

        //tables

        #region Shops

        public DbSet<Shop> Shops { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Courier> Couriers { get; set; }

        #endregion

        #region Products

        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductSerial> ProductSerials { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        #endregion

        #region Sales and Purchases

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<SaleState> SaleStates { get; set; }
        public DbSet<Installment> Installments { get; set; }

        #endregion

        #region Operation 

        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<OperationLogDetail> OperationLogDetails { get; set; }

        #endregion

        #region Customers

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Zone> Zones { get; set; }

        #endregion

        #region Accounts

        public DbSet<AccountHead> AccountHeads { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountInfo> AccountInfos { get; set; }

        #endregion

        #region Dealers

        public DbSet<Dealer> Dealers { get; set; }

        #endregion

        #region Message

        public DbSet<Sms> Smses { get; set; }
        public DbSet<HookDetail> HookDeatils { get; set; }
        public DbSet<SmsHook> SmsHooks { get; set; }
        public DbSet<SmsHistory> SmsHistorys { get; set; }

        #endregion

        #region DealerProduct

        public DbSet<DealerProduct> DealerProducts { get; set; }

        public DbSet<DealerProductTransaction> DealerProductTransactions { get; set; }

        #endregion

        #region SupplierProduct

        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<SupplierProductTransaction> SupplierProductTransactions { get; set; }

        #endregion

        #region EmployeeInfo

        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }

        #endregion

        #region Warehouse

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<StockTransfer> StockTransfers { get; set; }

        public DbSet<StockTransferDetail> StockTransferDetails { get; set; }

        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }

        public DbSet<WarehouseZone> WarehouseZones { get; set; }
        public DbSet<Damage> Damages { get; set; }

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BusinessDbContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}