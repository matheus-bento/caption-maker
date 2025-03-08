namespace CaptionMaker.Service.ImageStorage
{
    /// <summary>
    ///     Describes a service that can manipulate an image storage
    /// </summary>
    public interface IImageStorageService
    {
        /// <summary>
        ///     Retrieves an image saved in the storage with the specified filename
        /// </summary>
        /// <param name="filename">The filename that will be used to search in the storage</param>
        Task<Stream> GetAsync(string filename);

        /// <summary>
        ///     Saves an image into the storage
        /// </summary>
        /// <param name="image"><see cref="Stream"/> containing the image that will be saved</param>
        /// <returns>The filename attributed by the storage to the image</returns>
        Task<string> SaveAsync(Stream imageStream);
    }
}
