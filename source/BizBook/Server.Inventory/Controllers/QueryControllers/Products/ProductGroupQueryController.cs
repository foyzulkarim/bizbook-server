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
using ViewModel.Products;
using Rm = RequestModel.Products.ProductGroupRequestModel;
using M = Model.Products.ProductGroup;
using Vm = ViewModel.Products.ProductGroupViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Products
{
    [RoutePrefix("api/ProductGroupQuery")]
    public class ProductGroupQueryController : BaseQueryController<M, Rm, Vm>
    {
        public ProductGroupQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }

       
    }
}
