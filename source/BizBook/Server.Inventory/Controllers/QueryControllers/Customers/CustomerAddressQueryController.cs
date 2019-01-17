using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Rm = RequestModel.Customers.AddressRequestModel;
using M = Model.Customers.Address;
using Vm = ViewModel.Customers.AddressViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Customers
{
    using System.IO;
    using System.Linq;

    using CsvHelper;

    using Server.Inventory.Models;

    [RoutePrefix("api/CustomerAddressQuery")]
    public class CustomerAddressQueryController : BaseQueryController<M, Rm, Vm>
    {
        public CustomerAddressQueryController() : base(new BaseService<M, Rm, Vm>(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }

        [AllowAnonymous]
        [Route("Locations")]
        [ActionName("Locations")]
        [HttpGet]
        public IHttpActionResult GetLocations()
        {
            string path = System.Web.HttpContext.Current.Request.MapPath("~\\Files\\locations.csv");
            CsvReader reader = CreateCsvReader(path);
            var locations = reader.GetRecords<Location>().ToList();
            return this.Ok(locations);
        }

        private static CsvReader CreateCsvReader(string filename)
        {
            string readToEnd = "";
            using (StreamReader reader = File.OpenText(filename))
            {
                readToEnd = reader.ReadToEnd();
            }

            TextReader textReader = new StringReader(readToEnd);
            CsvReader csv = new CsvReader(textReader);
            return csv;
        }
    }
}
