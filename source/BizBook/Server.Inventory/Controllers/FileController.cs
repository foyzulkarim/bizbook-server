using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Inventory.Controllers
{
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;

    [AllowAnonymous]
    [RoutePrefix("api/File")]
    public class FileController : ApiController
    {
        [HttpPost]
        [ActionName("UploadImage")]
        [Route("UploadImage")]
        public async Task<HttpResponseMessage> UploadImage()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var rootPath = "C:/images";
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                string folderName = provider.FormData.Get("folderName");
                string id = provider.FormData.Get("id");
                string type = provider.FormData.Get("type");
                string subPart = $"{folderName}/{id}/";
                string fullPath = $"{rootPath}/{subPart}";

                string destFileName = "";
                foreach (MultipartFileData fileData in provider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }

                    string fileName = type + ".jpeg";
                   // string fileName = fileData.Headers.ContentDisposition.FileName;
                    //if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    //{
                    //    fileName = fileName.Trim('"');
                    //}
                    //if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    //{
                    //    fileName = Path.GetFileName(fileName);
                    //}

                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    destFileName = Path.Combine(fullPath, fileName);
                    if (File.Exists(destFileName))
                    {
                        File.Delete(destFileName);
                    }

                    File.Move(fileData.LocalFileName, destFileName);
                    File.Delete(fileData.LocalFileName);
                }

                return Request.CreateResponse(HttpStatusCode.OK, destFileName);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [ActionName("GetImage")]
        [Route("GetImage")]
        public HttpResponseMessage GetImage(string folderName, string id, string name)
        {
            var rootPath = "C:/images";
            string subPart = $"{folderName}/{id}/";
            string fullPath = $"{rootPath}/{subPart}/{name}";
            HttpResponseMessage response = new HttpResponseMessage();
            if (File.Exists(fullPath))
            {                
                response.Content = new StreamContent(new FileStream(fullPath, FileMode.Open));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");             
            }

            return response;
        }
    }
}
