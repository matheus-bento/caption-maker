using System.Net.Http.Headers;
using CaptionMaker.Model;

namespace CaptionMaker.Service.ImageStorage
{
    public class HttpImageStorageService : IImageStorageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpImageStorageService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<Stream> GetAsync(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            HttpClient httpClient = this._httpClientFactory.CreateClient("ImageStorage");

            var stream = await httpClient.GetStreamAsync($"file/{filename}");
            return stream;
        }

        public async Task<string> SaveAsync(Stream imageStream)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream));

            HttpClient httpClient = this._httpClientFactory.CreateClient("ImageStorage");

            imageStream.Seek(0, SeekOrigin.Begin);

            HttpResponseMessage req = await httpClient.PutAsync("file", new StreamContent(imageStream));

            req.EnsureSuccessStatusCode();

            SaveFileSuccessResponse res = await req.Content.ReadFromJsonAsync<SaveFileSuccessResponse>();
            
            return res.Filename;
        }
    }
}
