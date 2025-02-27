using CaptionMaker.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CaptionMaker.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private CaptionContext _dbContext = null;

        public UserRepository(CaptionContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task SaveAsync(string username, string password, string salt)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            if (String.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            await this._dbContext.Users.AddAsync(new User
            {
                CreatedAt = DateTime.Now,

                Username = username,
                Password = password,
                Salt = salt
            });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await this._dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UsernameAlreadyExistsAsync(string username)
        {
            return await this._dbContext.Users.Where(u => u.Username == username).CountAsync() > 0;
        }
    }
}
