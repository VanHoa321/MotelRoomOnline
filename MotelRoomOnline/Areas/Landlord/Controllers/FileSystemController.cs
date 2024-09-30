using elFinder.NetCore.Drivers.FileSystem;
using elFinder.NetCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    [Route("/Landlord/el-finder-file-system")]
    public class FileSystemController : Controller
    {
        readonly IWebHostEnvironment _env;
        public FileSystemController(IWebHostEnvironment env) => _env = env;
        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            var connector = GetConnector();
            var result = await connector.ProcessAsync(Request);
            if (result is JsonResult)
            {
                var json = result as JsonResult;
                return Content(JsonSerializer.Serialize(json.Value), json.ContentType);
            }
            else
            {
                return Json(result);
            }
        }

        // Địa chỉ để truy vấn thumbnail
        // /el-finder-file-system/thumb
        [Route("/file-manager/{hash}")]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            // Thư mục gốc lưu trữ là wwwwroot/files (đảm bảo có tạo thư mục này)
            string pathroot = "files_landlord";

            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            string rootDirectory = Path.Combine(_env.WebRootPath, pathroot);

            string url = $"/{pathroot}/";
            string urlthumb = $"{uri.Scheme}://{uri.Authority}/file-manager/";

            var root = new RootVolume(rootDirectory, url, urlthumb)
            {
                IsReadOnly = false,
                IsLocked = false,
                Alias = "Files",
                ThumbnailSize = 100,
            };


            driver.AddRoot(root);

            return new Connector(driver)
            {
                MimeDetect = MimeDetectOption.Internal
            };
        }
    }
}
