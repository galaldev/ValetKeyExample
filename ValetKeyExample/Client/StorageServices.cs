using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Web;
using ValetKeyExample.Shared;

namespace ValetKeyExample.Client
{
    public class StorageService
    {
        private readonly HttpClient _http;

        public StorageService(HttpClient http)
        {
            _http = http;
        }

        public Task<string> GetPresiginedURL(string fieName)
        {
            return _http.GetStringAsync($"storage/getkey?fileName={fieName}");
        }
        public async Task UploadFile(string url, MemoryStream stream)
        {
            var fileContents = stream.ToArray();
            // var contentType = "application/octet-stream";
        

            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new ByteArrayContent(fileContents)
            };
            //  request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();


        }

        public Task NotifyServer(string fileName)
        {
            return _http.PostAsJsonAsync("storage/Notify", new NotificationDto { FileName = fileName });
        }
        public Task<List<FileDto>?> GetFiles()
        {
            return _http.GetFromJsonAsync<List<FileDto>?>("storage/GetFiles");
        }
    }
}
