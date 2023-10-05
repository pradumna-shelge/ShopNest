using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileuploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FileuploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {

                return BadRequest("No file uploaded.");
            }

           
            const long maxFileSize = 2 * 1024 * 1024; 
            if (file.Length > maxFileSize)
            {

                return BadRequest("File size exceeds 2MB limit.");
            }

          
            if (!IsSupportedFileType(file.ContentType) || !file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Invalid file type. Supported types are png, jpg, jpeg, bmp.");
            }

            string accessKey = _configuration["AWS:AccessKey"]??"No Data";
            string secretKey = _configuration["AWS:SecretKey"] ?? "No Data";
            string bucketName = _configuration["AWS:BucketName"] ?? "No Data";

            if((accessKey == "No Data") || (secretKey == "No Data") || (bucketName) == "No Data") {
                return BadRequest("credentials Not Found");
            }


            if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(bucketName))
                return StatusCode(500, "AWS credentials or bucket configuration missing.");

            using (var client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSouth1))
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var fileTransferUtility = new TransferUtility(client);
                var keyName = Guid.NewGuid().ToString();
                await fileTransferUtility.UploadAsync(memoryStream, bucketName, keyName);

                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    Expires = DateTime.Now.AddHours(1)
                };

                string imageUrl = client.GetPreSignedURL(urlRequest);
                return Ok(new { ImageUrl = imageUrl });
            }
        }

        private bool IsSupportedFileType(string contentType)
        {
           
            string[] supportedTypes = { "image/png", "image/jpeg", "image/jpg", "image/bmp" };

            return supportedTypes.Contains(contentType.ToLower());
        }



    }
}
