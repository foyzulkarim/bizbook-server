namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountHeads",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        AccountHeadType = c.Int(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.AccountHeadType)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        StreetAddress = c.String(),
                        Area = c.String(maxLength: 50),
                        Thana = c.String(maxLength: 50),
                        PostCode = c.String(maxLength: 50),
                        District = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        ContactPersonName = c.String(maxLength: 50),
                        ContactPersonPhone = c.String(maxLength: 20),
                        ContactPersonDesignation = c.String(maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Website = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Facebook = c.String(maxLength: 50),
                        Remarks = c.String(),
                        About = c.String(maxLength: 100),
                        RegistrationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ExpiryDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                        SubscriptionType = c.String(maxLength: 50),
                        TotalUsers = c.Int(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        HasDeliveryChain = c.Boolean(nullable: false),
                        WcUrl = c.String(maxLength: 50),
                        WcKey = c.String(maxLength: 50),
                        WcSecret = c.String(maxLength: 50),
                        WcVersion = c.String(),
                        WcWebhookSource = c.String(maxLength: 100),
                        LogoUrl = c.String(maxLength: 200),
                        IsShowOrderNumber = c.Boolean(nullable: false),
                        IsAutoAddToCart = c.Boolean(nullable: false),
                        DeliveryCharge = c.Single(nullable: false),
                        ReceiptName = c.String(maxLength: 50),
                        ChalanName = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ShopName")
                .Index(t => t.ContactPersonName)
                .Index(t => t.ContactPersonPhone)
                .Index(t => t.ContactPersonDesignation)
                .Index(t => t.Phone, name: "IX_ShopPhone")
                .Index(t => t.Website)
                .Index(t => t.Email)
                .Index(t => t.Facebook)
                .Index(t => t.WcUrl)
                .Index(t => t.WcKey)
                .Index(t => t.WcSecret)
                .Index(t => t.WcWebhookSource)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        ContactPersonName = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        MadeInCountry = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        BrandCode = c.String(maxLength: 20),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.ProductDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        Model = c.String(maxLength: 50),
                        Year = c.String(maxLength: 50),
                        BarCode = c.String(nullable: false, maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        HasUniqueSerial = c.Boolean(nullable: false),
                        HasWarrenty = c.Boolean(nullable: false),
                        SalePrice = c.Double(nullable: false),
                        DealerPrice = c.Double(nullable: false),
                        CostPrice = c.Double(nullable: false),
                        Type = c.String(maxLength: 50),
                        Color = c.String(maxLength: 50),
                        StartingInventory = c.Int(nullable: false),
                        Purchased = c.Double(nullable: false),
                        Sold = c.Double(nullable: false),
                        OnHand = c.Double(nullable: false),
                        MinimumStockToNotify = c.Int(nullable: false),
                        WcId = c.Int(nullable: false),
                        Permalink = c.String(maxLength: 50),
                        WcType = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        ShortDescription = c.String(maxLength: 50),
                        Tags = c.String(maxLength: 50),
                        WcCategoryId = c.Int(nullable: false),
                        RelatedIds = c.String(maxLength: 50),
                        WcVariationPermalink = c.String(maxLength: 50),
                        WcVariationId = c.Int(nullable: false),
                        WcVariationOption = c.String(maxLength: 50),
                        ProductCategoryId = c.String(nullable: false, maxLength: 128),
                        BrandId = c.String(nullable: false, maxLength: 128),
                        HasExpiryDate = c.Boolean(nullable: false),
                        ExpireInDays = c.Int(nullable: false),
                        IsRawProduct = c.Boolean(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.BarCode)
                .Index(t => t.ProductCode)
                .Index(t => t.WcId)
                .Index(t => t.WcCategoryId)
                .Index(t => t.WcVariationId)
                .Index(t => t.ProductCategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WcId = c.Int(nullable: false),
                        Src = c.String(nullable: false, maxLength: 500),
                        Name = c.String(maxLength: 50),
                        Alt = c.String(maxLength: 50),
                        Position = c.Int(),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        WcId = c.Int(nullable: false),
                        ProductGroupId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.WcId)
                .Index(t => t.ProductGroupId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.PurchaseDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        PurchaseId = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Double(nullable: false),
                        CostPricePerUnit = c.Double(nullable: false),
                        CostTotal = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        Payable = c.Double(nullable: false),
                        Remarks = c.String(maxLength: 50),
                        WarehouseId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.PurchaseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        OrderReferenceNumber = c.String(maxLength: 50),
                        ShippingAmount = c.Double(nullable: false),
                        ProductAmount = c.Double(nullable: false),
                        OtherAmount = c.Double(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        State = c.String(maxLength: 50),
                        ShippingProvider = c.String(maxLength: 50),
                        ShipmentTrackingNo = c.String(maxLength: 50),
                        SupplierId = c.String(nullable: false, maxLength: 128),
                        WarehouseId = c.String(maxLength: 128),
                        Remarks = c.String(maxLength: 500),
                        OrderDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.OrderNumber)
                .Index(t => t.SupplierId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        StreetAddress = c.String(maxLength: 100),
                        Area = c.String(maxLength: 50),
                        Thana = c.String(maxLength: 50),
                        PostCode = c.String(maxLength: 50),
                        District = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 50),
                        CompanyName = c.String(maxLength: 50),
                        ContactPersonName = c.String(maxLength: 100),
                        IsVerified = c.Boolean(nullable: false),
                        OrdersCount = c.Int(nullable: false),
                        ProductAmount = c.Double(nullable: false),
                        OtherAmount = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        TotalPaid = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        SupplierShopId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Shop_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Shops", t => t.SupplierShopId)
                .ForeignKey("dbo.Shops", t => t.Shop_Id)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.OrdersCount)
                .Index(t => t.ProductAmount)
                .Index(t => t.OtherAmount)
                .Index(t => t.TotalDiscount)
                .Index(t => t.TotalAmount)
                .Index(t => t.TotalPaid)
                .Index(t => t.TotalDue)
                .Index(t => t.SupplierShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified)
                .Index(t => t.Shop_Id);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        StreetAddress = c.String(),
                        Area = c.String(),
                        District = c.String(),
                        IsMain = c.Boolean(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WcId = c.Int(),
                        WcProductId = c.Int(),
                        WcProductVariationId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        CostPricePerUnit = c.Double(nullable: false),
                        CostTotal = c.Double(nullable: false),
                        ProductPricePerUnit = c.Double(nullable: false),
                        DiscountPerUnit = c.Double(nullable: false),
                        SalePricePerUnit = c.Double(nullable: false),
                        PriceTotal = c.Double(nullable: false),
                        DiscountTotal = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        ProductSerialNumber = c.String(maxLength: 50),
                        IsReturned = c.Boolean(nullable: false),
                        ReturnReason = c.String(maxLength: 200),
                        SaleId = c.String(nullable: false, maxLength: 128),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        WarehouseId = c.String(maxLength: 128),
                        Remarks = c.String(),
                        SaleDetailType = c.Int(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WcId)
                .Index(t => t.SaleId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        OrderReferenceNumber = c.String(maxLength: 50),
                        ProductAmount = c.Double(nullable: false),
                        DeliveryChargeAmount = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        PaymentServiceChargeAmount = c.Double(nullable: false),
                        OtherAmount = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        PayableTotalAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        CostAmount = c.Double(nullable: false),
                        ProfitAmount = c.Double(nullable: false),
                        ProfitPercent = c.Double(nullable: false),
                        PaidByCashAmount = c.Double(nullable: false),
                        PaidByOtherAmount = c.Double(nullable: false),
                        CourierShopId = c.String(maxLength: 128),
                        CourierName = c.String(maxLength: 50),
                        DeliveryTrackingNo = c.String(maxLength: 50),
                        DeliverymanId = c.String(maxLength: 50),
                        DeliverymanName = c.String(maxLength: 50),
                        DeliverymanPhone = c.String(maxLength: 50),
                        EstimatedDeliveryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RequiredDeliveryDateByCustomer = c.DateTime(precision: 7, storeType: "datetime2"),
                        RequiredDeliveryTimeByCustomer = c.String(maxLength: 50),
                        OrderDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CustomerId = c.String(maxLength: 128),
                        AddressId = c.String(maxLength: 128),
                        BillingAddressId = c.String(maxLength: 128),
                        CustomerArea = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerNote = c.String(),
                        Guarantor1Id = c.String(),
                        Guarantor2Id = c.String(),
                        IsDealerSale = c.Boolean(nullable: false),
                        DealerId = c.String(maxLength: 128),
                        Remarks = c.String(maxLength: 200),
                        Version = c.Int(nullable: false),
                        ParentSaleId = c.String(maxLength: 50),
                        SaleChannel = c.Int(nullable: false),
                        SaleFrom = c.Int(nullable: false),
                        OrderState = c.Int(nullable: false),
                        WcId = c.Int(),
                        WcCustomerId = c.Int(),
                        WcOrderKey = c.String(maxLength: 50),
                        WcCartHash = c.String(maxLength: 50),
                        WcOrderStatus = c.String(maxLength: 50),
                        EmployeeInfoName = c.String(maxLength: 50),
                        EmployeeInfoId = c.String(maxLength: 128),
                        WarehouseId = c.String(maxLength: 128),
                        IsTaggedSale = c.Boolean(nullable: false),
                        SaleTag = c.String(),
                        InstallmentId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId)
                .ForeignKey("dbo.Dealers", t => t.DealerId)
                .ForeignKey("dbo.EmployeeInfoes", t => t.EmployeeInfoId)
                .ForeignKey("dbo.Installments", t => t.InstallmentId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.OrderNumber)
                .Index(t => t.CourierShopId)
                .Index(t => t.CustomerId)
                .Index(t => t.AddressId)
                .Index(t => t.BillingAddressId)
                .Index(t => t.DealerId)
                .Index(t => t.SaleChannel)
                .Index(t => t.SaleFrom)
                .Index(t => t.OrderState)
                .Index(t => t.WcId)
                .Index(t => t.WcCustomerId)
                .Index(t => t.EmployeeInfoId)
                .Index(t => t.WarehouseId)
                .Index(t => t.InstallmentId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AddressName = c.String(maxLength: 50),
                        IsDefault = c.Boolean(nullable: false),
                        ContactName = c.String(maxLength: 100),
                        ContactPhone = c.String(maxLength: 100),
                        StreetAddress = c.String(maxLength: 500),
                        Area = c.String(maxLength: 50),
                        Thana = c.String(maxLength: 50),
                        PostCode = c.String(maxLength: 20),
                        District = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        SpecialNote = c.String(maxLength: 150),
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.CustomerId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MembershipCardNo = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 50),
                        NationalId = c.String(maxLength: 50),
                        ImageUrl = c.String(maxLength: 300),
                        Occupation = c.String(maxLength: 50),
                        University = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Point = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                        OrdersCount = c.Int(nullable: false),
                        ProductAmount = c.Double(nullable: false),
                        OtherAmount = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        TotalPaid = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        WcId = c.Int(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.MembershipCardNo)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.Email)
                .Index(t => t.NationalId)
                .Index(t => t.ImageUrl)
                .Index(t => t.Point)
                .Index(t => t.OrdersCount)
                .Index(t => t.ProductAmount)
                .Index(t => t.OtherAmount)
                .Index(t => t.TotalDiscount)
                .Index(t => t.TotalAmount)
                .Index(t => t.TotalPaid)
                .Index(t => t.TotalDue)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.CustomerFeedbacks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        OrderNumber = c.String(),
                        Feedback = c.String(maxLength: 500),
                        FeedbackType = c.Int(nullable: false),
                        ManagerComment = c.String(maxLength: 500),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.CustomerId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        StreetAddress = c.String(maxLength: 100),
                        Area = c.String(maxLength: 50),
                        Thana = c.String(maxLength: 50),
                        PostCode = c.String(maxLength: 50),
                        District = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 50),
                        CompanyName = c.String(maxLength: 50),
                        ContactPersonName = c.String(maxLength: 100),
                        IsVerified = c.Boolean(nullable: false),
                        OrdersCount = c.Int(nullable: false),
                        ProductAmount = c.Double(nullable: false),
                        OtherAmount = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        TotalPaid = c.Double(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.OrdersCount)
                .Index(t => t.ProductAmount)
                .Index(t => t.OtherAmount)
                .Index(t => t.TotalDiscount)
                .Index(t => t.TotalAmount)
                .Index(t => t.TotalPaid)
                .Index(t => t.TotalDue)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.EmployeeInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(maxLength: 100),
                        Email = c.String(maxLength: 50),
                        RoleId = c.String(),
                        Salary = c.Double(nullable: false),
                        SaleTargetAmount = c.Double(nullable: false),
                        SaleAchivedAmount = c.Double(nullable: false),
                        WarehouseId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.Name)
                .Index(t => t.Phone)
                .Index(t => t.Email)
                .Index(t => t.WarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Installments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CashPriceAmount = c.Double(nullable: false),
                        ProfitPercent = c.Double(nullable: false),
                        ProfitAmount = c.Double(nullable: false),
                        InstallmentTotalAmount = c.Double(nullable: false),
                        DownPaymentPercent = c.Double(nullable: false),
                        DownPaymentAmount = c.Double(nullable: false),
                        InstallmentDueAmount = c.Double(nullable: false),
                        InstallmentMonth = c.String(),
                        InstallmentPerMonthAmount = c.Double(nullable: false),
                        SaleType = c.Int(nullable: false),
                        CashPriceDueAmount = c.Double(nullable: false),
                        ProfitAmountPerMonth = c.Double(nullable: false),
                        SaleId = c.String(),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.InstallmentDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        InstallmentId = c.String(maxLength: 128),
                        SaleId = c.String(maxLength: 128),
                        TansactionId = c.String(maxLength: 128),
                        ScheduledAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        ScheduledDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PaidDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Installments", t => t.InstallmentId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Transactions", t => t.TansactionId)
                .Index(t => t.InstallmentId)
                .Index(t => t.SaleId)
                .Index(t => t.TansactionId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TransactionFlowType = c.Int(nullable: false),
                        TransactionMedium = c.Int(nullable: false),
                        PaymentGatewayService = c.Int(nullable: false),
                        TransactionFor = c.Int(nullable: false),
                        TransactionWith = c.Int(nullable: false),
                        ParentId = c.String(maxLength: 50),
                        ParentName = c.String(maxLength: 50),
                        OrderNumber = c.String(maxLength: 50),
                        OrderId = c.String(maxLength: 50),
                        Amount = c.Double(nullable: false),
                        TransactionMediumName = c.String(nullable: false, maxLength: 50),
                        PaymentGatewayServiceName = c.String(nullable: false, maxLength: 50),
                        TransactionNumber = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 50),
                        ContactPersonName = c.String(maxLength: 50),
                        ContactPersonPhone = c.String(maxLength: 50),
                        TransactionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AccountHeadName = c.String(nullable: false, maxLength: 50),
                        AccountHeadId = c.String(nullable: false, maxLength: 128),
                        WalletId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountHeads", t => t.AccountHeadId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Wallets", t => t.WalletId)
                .Index(t => t.TransactionFlowType)
                .Index(t => t.TransactionMedium)
                .Index(t => t.PaymentGatewayService)
                .Index(t => t.TransactionFor)
                .Index(t => t.TransactionWith)
                .Index(t => t.ParentId)
                .Index(t => t.ParentName)
                .Index(t => t.OrderNumber)
                .Index(t => t.OrderId)
                .Index(t => t.Amount)
                .Index(t => t.TransactionMediumName)
                .Index(t => t.PaymentGatewayServiceName)
                .Index(t => t.TransactionNumber)
                .Index(t => t.AccountHeadName)
                .Index(t => t.AccountHeadId)
                .Index(t => t.WalletId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountTitle = c.String(nullable: false, maxLength: 50),
                        AccountNumber = c.String(maxLength: 100),
                        BankName = c.String(maxLength: 50),
                        WalletType = c.Int(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.AccountTitle)
                .Index(t => t.WalletType)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SaleStates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        State = c.String(nullable: false, maxLength: 50),
                        Remarks = c.String(),
                        SaleId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.State)
                .Index(t => t.SaleId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Couriers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ContactPersonName = c.String(maxLength: 50),
                        CourierShopId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.CourierShopId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.CourierShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Damages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WarehouseId = c.String(maxLength: 128),
                        ProductDetailId = c.String(maxLength: 128),
                        Quantity = c.Double(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.DealerProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        Due = c.Double(nullable: false),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        DealerId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dealers", t => t.DealerId)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.DealerId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.DealerProductTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Amount = c.Double(nullable: false),
                        TransactionId = c.String(nullable: false, maxLength: 128),
                        DealerProductId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DealerProducts", t => t.DealerProductId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Transactions", t => t.TransactionId)
                .Index(t => t.TransactionId)
                .Index(t => t.DealerProductId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Name_Bn = c.String(),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        DistrictId = c.String(maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .Index(t => t.DistrictId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.HookDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SmsHookId = c.String(nullable: false, maxLength: 128),
                        HookName = c.String(nullable: false, maxLength: 200),
                        IsEnabled = c.Boolean(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.SmsHooks", t => t.SmsHookId)
                .Index(t => t.SmsHookId)
                .Index(t => t.HookName)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SmsHooks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BizSmsHook = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        IsEnabled = c.Boolean(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.OperationLogDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OperationType = c.Int(nullable: false),
                        ModelName = c.Int(nullable: false),
                        ObjectId = c.String(),
                        ObjectIdentifier = c.String(),
                        PropertyName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        Remarks = c.String(),
                        OperationLogId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OperationLogs", t => t.OperationLogId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.OperationLogId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.OperationLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OperationType = c.Int(nullable: false),
                        ModelName = c.Int(nullable: false),
                        ObjectId = c.String(),
                        ObjectIdentifier = c.String(),
                        Remarks = c.String(),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.ProductSerials",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SerialNumber = c.String(nullable: false, maxLength: 50),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        PurchaseDetailId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.PurchaseDetails", t => t.PurchaseDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.SerialNumber)
                .Index(t => t.ProductDetailId)
                .Index(t => t.PurchaseDetailId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.Sms",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(nullable: false, maxLength: 200),
                        SmsReceiverType = c.Int(nullable: false),
                        ReceiverNumber = c.String(maxLength: 20),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        ReasonType = c.Int(nullable: false),
                        ReasonId = c.String(nullable: false, maxLength: 128),
                        DeliveryStatus = c.String(nullable: false, maxLength: 100),
                        Ismasked = c.Boolean(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ReceiverNumber)
                .Index(t => t.ReceiverId)
                .Index(t => t.ReasonType)
                .Index(t => t.ReasonId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SmsHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Amount = c.Double(nullable: false),
                        SmsId = c.String(),
                        Text = c.String(),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.StockTransferDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Double(nullable: false),
                        SalePricePerUnit = c.Double(nullable: false),
                        PriceTotal = c.Double(nullable: false),
                        StockTransferId = c.String(nullable: false, maxLength: 128),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        SourceWarehouseId = c.String(maxLength: 128),
                        DestinationWarehouseId = c.String(maxLength: 128),
                        Remarks = c.String(maxLength: 200),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.DestinationWarehouseId)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.SourceWarehouseId)
                .ForeignKey("dbo.StockTransfers", t => t.StockTransferId)
                .Index(t => t.StockTransferId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.SourceWarehouseId)
                .Index(t => t.DestinationWarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.StockTransfers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        OrderReferenceNumber = c.String(maxLength: 50),
                        ProductAmount = c.Double(nullable: false),
                        DeliveryTrackingNo = c.String(maxLength: 50),
                        DeliverymanId = c.String(maxLength: 50),
                        DeliverymanName = c.String(maxLength: 50),
                        DeliverymanPhone = c.String(maxLength: 50),
                        EstimatedDeliveryDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SourceWarehouseId = c.String(maxLength: 128),
                        DestinationWarehouseId = c.String(maxLength: 128),
                        Remarks = c.String(maxLength: 200),
                        TransferState = c.Int(nullable: false),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.DestinationWarehouseId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.SourceWarehouseId)
                .Index(t => t.OrderNumber)
                .Index(t => t.SourceWarehouseId)
                .Index(t => t.DestinationWarehouseId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SupplierProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        Due = c.Double(nullable: false),
                        ProductDetailId = c.String(nullable: false, maxLength: 128),
                        SupplierId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.SupplierId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.SupplierProductTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Amount = c.Double(nullable: false),
                        TransactionId = c.String(nullable: false, maxLength: 128),
                        SupplierProductId = c.String(nullable: false, maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.SupplierProducts", t => t.SupplierProductId)
                .ForeignKey("dbo.Transactions", t => t.TransactionId)
                .Index(t => t.TransactionId)
                .Index(t => t.SupplierProductId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.WarehouseProducts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StartingInventory = c.Int(nullable: false),
                        Purchased = c.Double(nullable: false),
                        Sold = c.Double(nullable: false),
                        TransferredIn = c.Double(nullable: false),
                        TransferredOut = c.Double(nullable: false),
                        OnHand = c.Double(nullable: false),
                        MinimumStockToNotify = c.Int(nullable: false),
                        WarehouseId = c.String(maxLength: 128),
                        ProductDetailId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ProductDetailId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
            CreateTable(
                "dbo.WarehouseZones",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WarehouseId = c.String(maxLength: 128),
                        ZoneId = c.String(maxLength: 128),
                        ShopId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedFrom = c.String(nullable: false),
                        Modified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .ForeignKey("dbo.Zones", t => t.ZoneId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ZoneId)
                .Index(t => t.ShopId)
                .Index(t => t.Created)
                .Index(t => t.Modified);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseZones", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.WarehouseZones", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseZones", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.WarehouseProducts", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseProducts", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.WarehouseProducts", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.SupplierProductTransactions", "TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.SupplierProductTransactions", "SupplierProductId", "dbo.SupplierProducts");
            DropForeignKey("dbo.SupplierProductTransactions", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierProducts", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.SupplierProducts", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.StockTransferDetails", "StockTransferId", "dbo.StockTransfers");
            DropForeignKey("dbo.StockTransfers", "SourceWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StockTransfers", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.StockTransfers", "DestinationWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StockTransferDetails", "SourceWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StockTransferDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.StockTransferDetails", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.StockTransferDetails", "DestinationWarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.SmsHistories", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Sms", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductSerials", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductSerials", "PurchaseDetailId", "dbo.PurchaseDetails");
            DropForeignKey("dbo.ProductSerials", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.OperationLogDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.OperationLogs", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.OperationLogDetails", "OperationLogId", "dbo.OperationLogs");
            DropForeignKey("dbo.HookDetails", "SmsHookId", "dbo.SmsHooks");
            DropForeignKey("dbo.SmsHooks", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.HookDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Zones", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.DealerProductTransactions", "TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.DealerProductTransactions", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.DealerProductTransactions", "DealerProductId", "dbo.DealerProducts");
            DropForeignKey("dbo.DealerProducts", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.DealerProducts", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.DealerProducts", "DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Damages", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Damages", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Damages", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.Couriers", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Couriers", "CourierShopId", "dbo.Shops");
            DropForeignKey("dbo.AccountHeads", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Suppliers", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.Brands", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.SaleDetails", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.SaleDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Sales", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Sales", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.SaleStates", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.SaleStates", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleDetails", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "InstallmentId", "dbo.Installments");
            DropForeignKey("dbo.Installments", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.InstallmentDetails", "TansactionId", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "WalletId", "dbo.Wallets");
            DropForeignKey("dbo.Wallets", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Transactions", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Transactions", "AccountHeadId", "dbo.AccountHeads");
            DropForeignKey("dbo.InstallmentDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.InstallmentDetails", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.InstallmentDetails", "InstallmentId", "dbo.Installments");
            DropForeignKey("dbo.Sales", "EmployeeInfoId", "dbo.EmployeeInfoes");
            DropForeignKey("dbo.EmployeeInfoes", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.EmployeeInfoes", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Dealers", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Sales", "DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Sales", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Sales", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Customers", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.CustomerFeedbacks", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.SaleDetails", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.PurchaseDetails", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.PurchaseDetails", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Purchases", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Warehouses", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Suppliers", "SupplierShopId", "dbo.Shops");
            DropForeignKey("dbo.Suppliers", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Purchases", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Purchases", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.PurchaseDetails", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseDetails", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.ProductCategories", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductGroups", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductCategories", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductDetails", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductImages", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.ProductImages", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.ProductDetails", "BrandId", "dbo.Brands");
            DropIndex("dbo.WarehouseZones", new[] { "Modified" });
            DropIndex("dbo.WarehouseZones", new[] { "Created" });
            DropIndex("dbo.WarehouseZones", new[] { "ShopId" });
            DropIndex("dbo.WarehouseZones", new[] { "ZoneId" });
            DropIndex("dbo.WarehouseZones", new[] { "WarehouseId" });
            DropIndex("dbo.WarehouseProducts", new[] { "Modified" });
            DropIndex("dbo.WarehouseProducts", new[] { "Created" });
            DropIndex("dbo.WarehouseProducts", new[] { "ShopId" });
            DropIndex("dbo.WarehouseProducts", new[] { "ProductDetailId" });
            DropIndex("dbo.WarehouseProducts", new[] { "WarehouseId" });
            DropIndex("dbo.SupplierProductTransactions", new[] { "Modified" });
            DropIndex("dbo.SupplierProductTransactions", new[] { "Created" });
            DropIndex("dbo.SupplierProductTransactions", new[] { "ShopId" });
            DropIndex("dbo.SupplierProductTransactions", new[] { "SupplierProductId" });
            DropIndex("dbo.SupplierProductTransactions", new[] { "TransactionId" });
            DropIndex("dbo.SupplierProducts", new[] { "Modified" });
            DropIndex("dbo.SupplierProducts", new[] { "Created" });
            DropIndex("dbo.SupplierProducts", new[] { "ShopId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductDetailId" });
            DropIndex("dbo.StockTransfers", new[] { "Modified" });
            DropIndex("dbo.StockTransfers", new[] { "Created" });
            DropIndex("dbo.StockTransfers", new[] { "ShopId" });
            DropIndex("dbo.StockTransfers", new[] { "DestinationWarehouseId" });
            DropIndex("dbo.StockTransfers", new[] { "SourceWarehouseId" });
            DropIndex("dbo.StockTransfers", new[] { "OrderNumber" });
            DropIndex("dbo.StockTransferDetails", new[] { "Modified" });
            DropIndex("dbo.StockTransferDetails", new[] { "Created" });
            DropIndex("dbo.StockTransferDetails", new[] { "ShopId" });
            DropIndex("dbo.StockTransferDetails", new[] { "DestinationWarehouseId" });
            DropIndex("dbo.StockTransferDetails", new[] { "SourceWarehouseId" });
            DropIndex("dbo.StockTransferDetails", new[] { "ProductDetailId" });
            DropIndex("dbo.StockTransferDetails", new[] { "StockTransferId" });
            DropIndex("dbo.SmsHistories", new[] { "Modified" });
            DropIndex("dbo.SmsHistories", new[] { "Created" });
            DropIndex("dbo.SmsHistories", new[] { "ShopId" });
            DropIndex("dbo.Sms", new[] { "Modified" });
            DropIndex("dbo.Sms", new[] { "Created" });
            DropIndex("dbo.Sms", new[] { "ShopId" });
            DropIndex("dbo.Sms", new[] { "ReasonId" });
            DropIndex("dbo.Sms", new[] { "ReasonType" });
            DropIndex("dbo.Sms", new[] { "ReceiverId" });
            DropIndex("dbo.Sms", new[] { "ReceiverNumber" });
            DropIndex("dbo.ProductSerials", new[] { "Modified" });
            DropIndex("dbo.ProductSerials", new[] { "Created" });
            DropIndex("dbo.ProductSerials", new[] { "ShopId" });
            DropIndex("dbo.ProductSerials", new[] { "PurchaseDetailId" });
            DropIndex("dbo.ProductSerials", new[] { "ProductDetailId" });
            DropIndex("dbo.ProductSerials", new[] { "SerialNumber" });
            DropIndex("dbo.OperationLogs", new[] { "Modified" });
            DropIndex("dbo.OperationLogs", new[] { "Created" });
            DropIndex("dbo.OperationLogs", new[] { "ShopId" });
            DropIndex("dbo.OperationLogDetails", new[] { "Modified" });
            DropIndex("dbo.OperationLogDetails", new[] { "Created" });
            DropIndex("dbo.OperationLogDetails", new[] { "ShopId" });
            DropIndex("dbo.OperationLogDetails", new[] { "OperationLogId" });
            DropIndex("dbo.SmsHooks", new[] { "Modified" });
            DropIndex("dbo.SmsHooks", new[] { "Created" });
            DropIndex("dbo.SmsHooks", new[] { "ShopId" });
            DropIndex("dbo.HookDetails", new[] { "Modified" });
            DropIndex("dbo.HookDetails", new[] { "Created" });
            DropIndex("dbo.HookDetails", new[] { "ShopId" });
            DropIndex("dbo.HookDetails", new[] { "HookName" });
            DropIndex("dbo.HookDetails", new[] { "SmsHookId" });
            DropIndex("dbo.Zones", new[] { "Modified" });
            DropIndex("dbo.Zones", new[] { "Created" });
            DropIndex("dbo.Zones", new[] { "DistrictId" });
            DropIndex("dbo.Districts", new[] { "Modified" });
            DropIndex("dbo.Districts", new[] { "Created" });
            DropIndex("dbo.DealerProductTransactions", new[] { "Modified" });
            DropIndex("dbo.DealerProductTransactions", new[] { "Created" });
            DropIndex("dbo.DealerProductTransactions", new[] { "ShopId" });
            DropIndex("dbo.DealerProductTransactions", new[] { "DealerProductId" });
            DropIndex("dbo.DealerProductTransactions", new[] { "TransactionId" });
            DropIndex("dbo.DealerProducts", new[] { "Modified" });
            DropIndex("dbo.DealerProducts", new[] { "Created" });
            DropIndex("dbo.DealerProducts", new[] { "ShopId" });
            DropIndex("dbo.DealerProducts", new[] { "DealerId" });
            DropIndex("dbo.DealerProducts", new[] { "ProductDetailId" });
            DropIndex("dbo.Damages", new[] { "Modified" });
            DropIndex("dbo.Damages", new[] { "Created" });
            DropIndex("dbo.Damages", new[] { "ShopId" });
            DropIndex("dbo.Damages", new[] { "ProductDetailId" });
            DropIndex("dbo.Damages", new[] { "WarehouseId" });
            DropIndex("dbo.Couriers", new[] { "Modified" });
            DropIndex("dbo.Couriers", new[] { "Created" });
            DropIndex("dbo.Couriers", new[] { "ShopId" });
            DropIndex("dbo.Couriers", new[] { "CourierShopId" });
            DropIndex("dbo.SaleStates", new[] { "Modified" });
            DropIndex("dbo.SaleStates", new[] { "Created" });
            DropIndex("dbo.SaleStates", new[] { "ShopId" });
            DropIndex("dbo.SaleStates", new[] { "SaleId" });
            DropIndex("dbo.SaleStates", new[] { "State" });
            DropIndex("dbo.Wallets", new[] { "Modified" });
            DropIndex("dbo.Wallets", new[] { "Created" });
            DropIndex("dbo.Wallets", new[] { "ShopId" });
            DropIndex("dbo.Wallets", new[] { "WalletType" });
            DropIndex("dbo.Wallets", new[] { "AccountTitle" });
            DropIndex("dbo.Transactions", new[] { "Modified" });
            DropIndex("dbo.Transactions", new[] { "Created" });
            DropIndex("dbo.Transactions", new[] { "ShopId" });
            DropIndex("dbo.Transactions", new[] { "WalletId" });
            DropIndex("dbo.Transactions", new[] { "AccountHeadId" });
            DropIndex("dbo.Transactions", new[] { "AccountHeadName" });
            DropIndex("dbo.Transactions", new[] { "TransactionNumber" });
            DropIndex("dbo.Transactions", new[] { "PaymentGatewayServiceName" });
            DropIndex("dbo.Transactions", new[] { "TransactionMediumName" });
            DropIndex("dbo.Transactions", new[] { "Amount" });
            DropIndex("dbo.Transactions", new[] { "OrderId" });
            DropIndex("dbo.Transactions", new[] { "OrderNumber" });
            DropIndex("dbo.Transactions", new[] { "ParentName" });
            DropIndex("dbo.Transactions", new[] { "ParentId" });
            DropIndex("dbo.Transactions", new[] { "TransactionWith" });
            DropIndex("dbo.Transactions", new[] { "TransactionFor" });
            DropIndex("dbo.Transactions", new[] { "PaymentGatewayService" });
            DropIndex("dbo.Transactions", new[] { "TransactionMedium" });
            DropIndex("dbo.Transactions", new[] { "TransactionFlowType" });
            DropIndex("dbo.InstallmentDetails", new[] { "Modified" });
            DropIndex("dbo.InstallmentDetails", new[] { "Created" });
            DropIndex("dbo.InstallmentDetails", new[] { "ShopId" });
            DropIndex("dbo.InstallmentDetails", new[] { "TansactionId" });
            DropIndex("dbo.InstallmentDetails", new[] { "SaleId" });
            DropIndex("dbo.InstallmentDetails", new[] { "InstallmentId" });
            DropIndex("dbo.Installments", new[] { "Modified" });
            DropIndex("dbo.Installments", new[] { "Created" });
            DropIndex("dbo.Installments", new[] { "ShopId" });
            DropIndex("dbo.EmployeeInfoes", new[] { "Modified" });
            DropIndex("dbo.EmployeeInfoes", new[] { "Created" });
            DropIndex("dbo.EmployeeInfoes", new[] { "ShopId" });
            DropIndex("dbo.EmployeeInfoes", new[] { "WarehouseId" });
            DropIndex("dbo.EmployeeInfoes", new[] { "Email" });
            DropIndex("dbo.EmployeeInfoes", new[] { "Phone" });
            DropIndex("dbo.EmployeeInfoes", new[] { "Name" });
            DropIndex("dbo.Dealers", new[] { "Modified" });
            DropIndex("dbo.Dealers", new[] { "Created" });
            DropIndex("dbo.Dealers", new[] { "ShopId" });
            DropIndex("dbo.Dealers", new[] { "TotalDue" });
            DropIndex("dbo.Dealers", new[] { "TotalPaid" });
            DropIndex("dbo.Dealers", new[] { "TotalAmount" });
            DropIndex("dbo.Dealers", new[] { "TotalDiscount" });
            DropIndex("dbo.Dealers", new[] { "OtherAmount" });
            DropIndex("dbo.Dealers", new[] { "ProductAmount" });
            DropIndex("dbo.Dealers", new[] { "OrdersCount" });
            DropIndex("dbo.Dealers", new[] { "Phone" });
            DropIndex("dbo.Dealers", new[] { "Name" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "Modified" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "Created" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "ShopId" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "Modified" });
            DropIndex("dbo.Customers", new[] { "Created" });
            DropIndex("dbo.Customers", new[] { "ShopId" });
            DropIndex("dbo.Customers", new[] { "TotalDue" });
            DropIndex("dbo.Customers", new[] { "TotalPaid" });
            DropIndex("dbo.Customers", new[] { "TotalAmount" });
            DropIndex("dbo.Customers", new[] { "TotalDiscount" });
            DropIndex("dbo.Customers", new[] { "OtherAmount" });
            DropIndex("dbo.Customers", new[] { "ProductAmount" });
            DropIndex("dbo.Customers", new[] { "OrdersCount" });
            DropIndex("dbo.Customers", new[] { "Point" });
            DropIndex("dbo.Customers", new[] { "ImageUrl" });
            DropIndex("dbo.Customers", new[] { "NationalId" });
            DropIndex("dbo.Customers", new[] { "Email" });
            DropIndex("dbo.Customers", new[] { "Phone" });
            DropIndex("dbo.Customers", new[] { "Name" });
            DropIndex("dbo.Customers", new[] { "MembershipCardNo" });
            DropIndex("dbo.Addresses", new[] { "Modified" });
            DropIndex("dbo.Addresses", new[] { "Created" });
            DropIndex("dbo.Addresses", new[] { "ShopId" });
            DropIndex("dbo.Addresses", new[] { "CustomerId" });
            DropIndex("dbo.Sales", new[] { "Modified" });
            DropIndex("dbo.Sales", new[] { "Created" });
            DropIndex("dbo.Sales", new[] { "ShopId" });
            DropIndex("dbo.Sales", new[] { "InstallmentId" });
            DropIndex("dbo.Sales", new[] { "WarehouseId" });
            DropIndex("dbo.Sales", new[] { "EmployeeInfoId" });
            DropIndex("dbo.Sales", new[] { "WcCustomerId" });
            DropIndex("dbo.Sales", new[] { "WcId" });
            DropIndex("dbo.Sales", new[] { "OrderState" });
            DropIndex("dbo.Sales", new[] { "SaleFrom" });
            DropIndex("dbo.Sales", new[] { "SaleChannel" });
            DropIndex("dbo.Sales", new[] { "DealerId" });
            DropIndex("dbo.Sales", new[] { "BillingAddressId" });
            DropIndex("dbo.Sales", new[] { "AddressId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            DropIndex("dbo.Sales", new[] { "CourierShopId" });
            DropIndex("dbo.Sales", new[] { "OrderNumber" });
            DropIndex("dbo.SaleDetails", new[] { "Modified" });
            DropIndex("dbo.SaleDetails", new[] { "Created" });
            DropIndex("dbo.SaleDetails", new[] { "ShopId" });
            DropIndex("dbo.SaleDetails", new[] { "WarehouseId" });
            DropIndex("dbo.SaleDetails", new[] { "ProductDetailId" });
            DropIndex("dbo.SaleDetails", new[] { "SaleId" });
            DropIndex("dbo.SaleDetails", new[] { "WcId" });
            DropIndex("dbo.Warehouses", new[] { "Modified" });
            DropIndex("dbo.Warehouses", new[] { "Created" });
            DropIndex("dbo.Warehouses", new[] { "ShopId" });
            DropIndex("dbo.Suppliers", new[] { "Shop_Id" });
            DropIndex("dbo.Suppliers", new[] { "Modified" });
            DropIndex("dbo.Suppliers", new[] { "Created" });
            DropIndex("dbo.Suppliers", new[] { "ShopId" });
            DropIndex("dbo.Suppliers", new[] { "SupplierShopId" });
            DropIndex("dbo.Suppliers", new[] { "TotalDue" });
            DropIndex("dbo.Suppliers", new[] { "TotalPaid" });
            DropIndex("dbo.Suppliers", new[] { "TotalAmount" });
            DropIndex("dbo.Suppliers", new[] { "TotalDiscount" });
            DropIndex("dbo.Suppliers", new[] { "OtherAmount" });
            DropIndex("dbo.Suppliers", new[] { "ProductAmount" });
            DropIndex("dbo.Suppliers", new[] { "OrdersCount" });
            DropIndex("dbo.Suppliers", new[] { "Phone" });
            DropIndex("dbo.Suppliers", new[] { "Name" });
            DropIndex("dbo.Purchases", new[] { "Modified" });
            DropIndex("dbo.Purchases", new[] { "Created" });
            DropIndex("dbo.Purchases", new[] { "ShopId" });
            DropIndex("dbo.Purchases", new[] { "WarehouseId" });
            DropIndex("dbo.Purchases", new[] { "SupplierId" });
            DropIndex("dbo.Purchases", new[] { "OrderNumber" });
            DropIndex("dbo.PurchaseDetails", new[] { "Modified" });
            DropIndex("dbo.PurchaseDetails", new[] { "Created" });
            DropIndex("dbo.PurchaseDetails", new[] { "ShopId" });
            DropIndex("dbo.PurchaseDetails", new[] { "WarehouseId" });
            DropIndex("dbo.PurchaseDetails", new[] { "PurchaseId" });
            DropIndex("dbo.PurchaseDetails", new[] { "ProductDetailId" });
            DropIndex("dbo.ProductGroups", new[] { "Modified" });
            DropIndex("dbo.ProductGroups", new[] { "Created" });
            DropIndex("dbo.ProductGroups", new[] { "ShopId" });
            DropIndex("dbo.ProductGroups", new[] { "Name" });
            DropIndex("dbo.ProductCategories", new[] { "Modified" });
            DropIndex("dbo.ProductCategories", new[] { "Created" });
            DropIndex("dbo.ProductCategories", new[] { "ShopId" });
            DropIndex("dbo.ProductCategories", new[] { "ProductGroupId" });
            DropIndex("dbo.ProductCategories", new[] { "WcId" });
            DropIndex("dbo.ProductCategories", new[] { "Name" });
            DropIndex("dbo.ProductImages", new[] { "Modified" });
            DropIndex("dbo.ProductImages", new[] { "Created" });
            DropIndex("dbo.ProductImages", new[] { "ShopId" });
            DropIndex("dbo.ProductImages", new[] { "ProductDetailId" });
            DropIndex("dbo.ProductDetails", new[] { "Modified" });
            DropIndex("dbo.ProductDetails", new[] { "Created" });
            DropIndex("dbo.ProductDetails", new[] { "ShopId" });
            DropIndex("dbo.ProductDetails", new[] { "BrandId" });
            DropIndex("dbo.ProductDetails", new[] { "ProductCategoryId" });
            DropIndex("dbo.ProductDetails", new[] { "WcVariationId" });
            DropIndex("dbo.ProductDetails", new[] { "WcCategoryId" });
            DropIndex("dbo.ProductDetails", new[] { "WcId" });
            DropIndex("dbo.ProductDetails", new[] { "ProductCode" });
            DropIndex("dbo.ProductDetails", new[] { "BarCode" });
            DropIndex("dbo.ProductDetails", new[] { "Name" });
            DropIndex("dbo.Brands", new[] { "Modified" });
            DropIndex("dbo.Brands", new[] { "Created" });
            DropIndex("dbo.Brands", new[] { "ShopId" });
            DropIndex("dbo.Brands", new[] { "Name" });
            DropIndex("dbo.Shops", new[] { "Modified" });
            DropIndex("dbo.Shops", new[] { "Created" });
            DropIndex("dbo.Shops", new[] { "WcWebhookSource" });
            DropIndex("dbo.Shops", new[] { "WcSecret" });
            DropIndex("dbo.Shops", new[] { "WcKey" });
            DropIndex("dbo.Shops", new[] { "WcUrl" });
            DropIndex("dbo.Shops", new[] { "Facebook" });
            DropIndex("dbo.Shops", new[] { "Email" });
            DropIndex("dbo.Shops", new[] { "Website" });
            DropIndex("dbo.Shops", "IX_ShopPhone");
            DropIndex("dbo.Shops", new[] { "ContactPersonDesignation" });
            DropIndex("dbo.Shops", new[] { "ContactPersonPhone" });
            DropIndex("dbo.Shops", new[] { "ContactPersonName" });
            DropIndex("dbo.Shops", "IX_ShopName");
            DropIndex("dbo.AccountHeads", new[] { "Modified" });
            DropIndex("dbo.AccountHeads", new[] { "Created" });
            DropIndex("dbo.AccountHeads", new[] { "ShopId" });
            DropIndex("dbo.AccountHeads", new[] { "AccountHeadType" });
            DropIndex("dbo.AccountHeads", new[] { "Name" });
            DropTable("dbo.WarehouseZones");
            DropTable("dbo.WarehouseProducts");
            DropTable("dbo.SupplierProductTransactions");
            DropTable("dbo.SupplierProducts");
            DropTable("dbo.StockTransfers");
            DropTable("dbo.StockTransferDetails");
            DropTable("dbo.SmsHistories");
            DropTable("dbo.Sms");
            DropTable("dbo.ProductSerials");
            DropTable("dbo.OperationLogs");
            DropTable("dbo.OperationLogDetails");
            DropTable("dbo.SmsHooks");
            DropTable("dbo.HookDetails");
            DropTable("dbo.Zones");
            DropTable("dbo.Districts");
            DropTable("dbo.DealerProductTransactions");
            DropTable("dbo.DealerProducts");
            DropTable("dbo.Damages");
            DropTable("dbo.Couriers");
            DropTable("dbo.SaleStates");
            DropTable("dbo.Wallets");
            DropTable("dbo.Transactions");
            DropTable("dbo.InstallmentDetails");
            DropTable("dbo.Installments");
            DropTable("dbo.EmployeeInfoes");
            DropTable("dbo.Dealers");
            DropTable("dbo.CustomerFeedbacks");
            DropTable("dbo.Customers");
            DropTable("dbo.Addresses");
            DropTable("dbo.Sales");
            DropTable("dbo.SaleDetails");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseDetails");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.ProductImages");
            DropTable("dbo.ProductDetails");
            DropTable("dbo.Brands");
            DropTable("dbo.Shops");
            DropTable("dbo.AccountHeads");
        }
    }
}
