
using Microsoft.AspNetCore.Mvc;
using Services.Models.DTO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Http;
using Services.Models;

namespace Application.Areas.V1.Controllers
{
    public class FileController : BaseController
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromServices] AppSettings settings)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string fileUrl = "";

            if (file != null)
                fileUrl =  UploadFile(file, "uploads", Guid.NewGuid());


            return Ok(new AppResult().Good("",$"{settings.ApplicationUrl}{fileUrl}"));
        }

    }
}
