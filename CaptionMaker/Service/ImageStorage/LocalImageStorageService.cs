namespace CaptionMaker.Service.ImageStorage
{
    /// <summary>
    ///     Class with methods to access the image storage inside the application server
    /// </summary>
    public class LocalImageStorageService : IImageStorageService
    {
        private readonly string _baseFilepath = "/usr/share/www/static/";

        public async Task<Stream> GetAsync(string filename)
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(this._baseFilepath + filename))
                    throw new FileNotFoundException();

                return File.OpenRead(this._baseFilepath + filename);
            });
        }

        public async Task<string> SaveAsync(Stream imageStream)
        {
            string filename = Guid.NewGuid().ToString();

            using (var fileStream = File.Create(this._baseFilepath + filename))
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                await imageStream.CopyToAsync(fileStream);
            }

            return filename;
        }
    }
}
