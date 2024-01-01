using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Easy_crud.CommonLayer.Model;
using Easy_crud.DataAccessLayer;


namespace Easy_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        public readonly IUploadFileDL _uploadFileDL;
        public UploadFileController(IUploadFileDL uploadFileDL)
        {
            _uploadFileDL = uploadFileDL;
        }

        [HttpPost]
        [Route(template: "UploadExcelFile")]
        public async Task<IActionResult> UploadExcelFile([FromForm] UploadExcelFileRequest request)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            string path = "UploadFileFolder/" + request.File.FileName;
            try
            {

                using (FileStream stream = new FileStream(path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }

                response = await _uploadFileDL.UploadExcelFile(request, path);

                string[] files = Directory.GetFiles("UploadFileFolder/");
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                    Console.WriteLine($"{file} is deleted.");
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }

            return Ok(response);
        }

    }
}
