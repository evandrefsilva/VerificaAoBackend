using Services;
using Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Areas.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        [NonAction]
        public string UploadDocument(IFormFile file, string folder, Guid id)
        {
            var fileName = id.ToString();
            var filePathVirtual = "/Storage" + "/" + folder + "/" + fileName;
            var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Storage", folder);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            using (
                var stream = System.IO.File.Create(Path.Combine(filePath, fileName)))
            {
                file.CopyTo(stream);
            }
            return filePathVirtual + Path.GetExtension(file.FileName);
        }
        [NonAction]
        public string UploadDocument(IFormFile file, string folder, string id)
        {
            try
            {
                var fileName = id.ToString() + Path.GetExtension(file.FileName);
                var filePathVirtual = "/Storage" + "/" + folder + "/" + fileName;
                var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Storage", folder);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                using (
                    var stream = System.IO.File.Create(Path.Combine(filePath, fileName)))
                {
                    file.CopyTo(stream);
                }
                return filePathVirtual;

            }
            catch (Exception)
            {
                return null;
            }
            }
        [NonAction]
        public string DeleteFile(string UrlFile)
        {
            List<string> list = new List<string>() { Environment.CurrentDirectory, "wwwroot" };
            list.AddRange(UrlFile.Split("/").ToList());
            var filePath = Path.Combine(list.ToArray());
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            return filePath;

        }
    }
}
