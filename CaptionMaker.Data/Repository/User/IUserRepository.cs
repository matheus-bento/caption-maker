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

        /// <summary>
        ///     Determines if a given username already exists in the database
        /// </summary>
        Task<bool> UsernameAlreadyExistsAsync(string username);
    }
}
