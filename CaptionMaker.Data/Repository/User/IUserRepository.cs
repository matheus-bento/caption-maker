using CaptionMaker.Data.Model;

namespace CaptionMaker.Data.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        ///     Registers a new user into the database
        /// </summary>
        Task SaveAsync(string username, string password, string salt);

        /// <summary>
        ///     Gets the data for the user with the specified username
        /// </summary>
        Task<User> GetByUsernameAsync(string username);
    }
}
