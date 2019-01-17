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
using RequestModel.Shops;
using ViewModel.Shops;
using M = Model.Shops.Brand;

namespace Server.Inventory.Controllers.QueryControllers.Shops
{
    [RoutePrefix("api/BrandQuery")]
    public class BrandQueryController : BaseQueryController<M, BrandRequestModel, BrandViewModel>
    {
        public BrandQueryController() : base(new BaseService<M, BrandRequestModel, BrandViewModel>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }

       
    }
}