using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using ValetKeyExample.Shared;

namespace ValetKeyExample.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StorageController : ControllerBase
    {
        private const string AccessKey = "UrZ3Jy3Q0y8mn6eI";
        private const string SecretKey = "ZoYxXAcuh0eLPkF8SqxDlJwBY5s5gkkM";
        private const string ServiceURL = "http://127.0.0.1:9000";
        private const string BucketName = "test2";

        static List<string> _fileNames = new List<string>();

        [HttpGet]
        public ActionResult GetKey([FromQuery] string fileName)
        {
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, new AmazonS3Config
            {
                ServiceURL = ServiceURL,
            });
            var url = client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = $"Uploads/{fileName}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(1),
                Protocol = Protocol.HTTP,
            });
            return Ok(url);
        }
        [HttpGet]
        public ActionResult GetFiles()
        {
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, new AmazonS3Config
            {
                ServiceURL = ServiceURL
            });

            var result = _fileNames.Select(fileName => new FileDto
            {
                Name = fileName,
                Url = client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = BucketName,
                    Key = $"Uploads/{fileName}",
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Protocol = Protocol.HTTP,
                })
            }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public ActionResult Notify([FromBody] NotificationDto notification)
        {
            //Save file name in DB
            _fileNames.Add(notification.FileName);

            return Ok();
        }

    }
}