using FlightDoc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightDoc.RestController.UploadAndDown_DocumentFlight
{
    [Route("api/v1/account/manageFile")]
    public class FileController : ControllerBase
    {
        // địa chỉ tải file mặc định của hệ thống
        private string fileDirectory = "C:\\Users\\openw\\Desktop\\FlightDoc\\Uploads\\StaticContent\\";


        [Authorize(Policy = "ReadFilePolicy")]
        [HttpGet("view/{fileName}")]
    public IActionResult ViewFile(string fileName)
    {
    try
    {
        string filePath = Path.Combine(fileDirectory, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("khong tim thay file");
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        return File("view thanh cong file len form " + fileBytes, contentType);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
    }


        [Authorize(Policy = "UpFilePolicy")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                var a = DateTime.Now.ToString();
                string uniqueFileName =  "DocFight_" + file.FileName;

                string filePath = Path.Combine("http://localhost:5000/uploads/", uniqueFileName);
              
               
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save filePath to the database or perform any other necessary operations

                return Ok(a + " upload thanh cong  " + uniqueFileName );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // private string fileDownloadUrl = "http://localhost:5000/api/files/download";
        // lấy file từ hệ thống
        [Authorize(Policy = "DownloadFilePolicy")]
        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFileSystem(string fileName, [FromQuery] string savePath)
        {
            try
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    return BadRequest("Missing savePath parameter.");
                }

                string fileUrl = $"http://localhost:5000/uploads/{fileName}";

                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(fileUrl).Result;
                    var fileBytes = response.Content.ReadAsByteArrayAsync().Result;
                    var saveFilePath = Path.Combine(savePath, fileName);
                    System.IO.File.WriteAllBytes(saveFilePath, fileBytes);

                    var contentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    };
                    Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                    return PhysicalFile(saveFilePath, "application/octet-stream");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [Authorize(Policy = "DownloadFilePolicy")]
        [HttpGet("custom/download")]
        public IActionResult DownloadFile([FromQuery] string fileUrl, [FromQuery] string savePath)
        {
            try
            {
                // Kiểm tra nếu đường dẫn file và đường dẫn lưu file tải về không được truyền
                if (string.IsNullOrEmpty(fileUrl) || string.IsNullOrEmpty(savePath))
                {
                    return BadRequest("Thiếu đường dẫn tải file và lưu file!");
                }

                // Tải file từ đường dẫn fileUrl
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(fileUrl).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("tải xuống file thất bại");
                    }

                    var fileBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // Lưu file tải về vào đường dẫn savePath
                    var saveFilePath = Path.Combine(savePath, Path.GetFileName(fileUrl));
                    System.IO.File.WriteAllBytes(saveFilePath, fileBytes);

                    // Trả về file đã tải về
                    return PhysicalFile(saveFilePath, "application/octet-stream", Path.GetFileName(fileUrl));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin", Policy = "DownloadFilePolicy") ]
        [HttpDelete("delete/{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                string filePath = Path.Combine(fileDirectory, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                System.IO.File.Delete(filePath);

                // Perform any additional cleanup or database operations

                return Ok("xoa file thanh cong! ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }

}
