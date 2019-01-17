using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Products;
using RequestModel.Products;
using ViewModel.Products;

namespace Server.Inventory.Controllers.QueryControllers.Products
{
    [RoutePrefix("api/ProductCategoryQuery")]
    public class ProductCategoryQueryController : BaseQueryController<ProductCategory, ProductCategoryRequestModel, ProductCategoryViewModel>
    {

        public ProductCategoryQueryController() : base(new BaseService<ProductCategory, ProductCategoryRequestModel, ProductCategoryViewModel>(new BaseRepository<ProductCategory>(BusinessDbContext.Create())))
        {
        }

         
    }
}
