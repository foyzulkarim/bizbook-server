using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Aspose.BarCode;
using CommonLibrary.Repository;
using Model;
using Model.Products;
using ServiceLibrary.Products;

namespace Server.Inventory.Controllers
{
    [RoutePrefix("api/BarcodeImage")]
    public class BarcodeImageController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("Download")]
        [ActionName("Download")]
        public HttpResponseMessage GetByProductId(string id)
        {
            string uploadPath = "";
            //uploadPath = HttpContext.Current.Server.MapPath("~/BarcodeImages");
            //uploadPath = ConfigurationManager.AppSettings["BarcodeImages"];
            uploadPath = Path.GetTempPath();
            bool exists = Directory.Exists(uploadPath);
            if (!exists)
                Directory.CreateDirectory(uploadPath);
            string filename = uploadPath + "/" + id + ".png";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            BusinessDbContext businessDbContext = BusinessDbContext.Create();
            var repository = new BaseRepository<ProductDetail>(businessDbContext);
            var service = new ProductDetailService(repository);
            var model = service.GetById(id);
            BarCodeBuilder builder = new BarCodeBuilder(model.BarCode);
            string text = $"{model.Name} \n MRP: {model.SalePrice} Tk";
            Caption captionBelow = new Caption(text)
            {
                TextAlign = StringAlignment.Center,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold)
            };
            builder.CaptionBelow = captionBelow;

            builder.Save(filename, ImageFormat.Png);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            //Stream stream = new FileStream(filename, FileMode.Open);
            Stream stream = new MemoryStream(File.ReadAllBytes(filename));
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            return response;
        }
    }
}
