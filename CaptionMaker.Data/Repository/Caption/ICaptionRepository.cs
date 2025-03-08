using CaptionMaker.Data.Model;

namespace CaptionMaker.Data.Repository
{
    public interface ICaptionRepository
    {
        /// <summary>
        ///     Returns a list of all saved captions
        /// </summary>
        Task<IEnumerable<Caption>> ListAsync();

        /// <summary>
        ///     <para>Saves a caption into the database.</para>
        ///
        ///     <para>
        ///         This method does not save the image into the file storage, it only saves a reference to the image
        ///         so it can be retrieved in the future
        ///     </para>
        /// </summary>
        /// <param name="filename">The name of the generated image in the file system</param>
        /// <param name="username">The name of the user who generated the image</param>
        Task SaveAsync(string filename, string username);
    }
}
