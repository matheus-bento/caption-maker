using CaptionMaker.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CaptionMaker.Data.Repository
{
    public class CaptionRepository : ICaptionRepository
    {
        private readonly CaptionContext _dbContext;

        public CaptionRepository(CaptionContext captionContext)
        {
            this._dbContext = captionContext;
        }

        public async Task<IEnumerable<Caption>> ListAsync()
        {
            return await this._dbContext.Captions.Include(c => c.User).ToListAsync();
        }

        public async Task SaveAsync(string filename, string username)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            int? userId =
                (await this._dbContext
                    .Users
                    .FirstOrDefaultAsync(u => u.Username == username))
                    ?.Id;

            if (!userId.HasValue)
                throw new Exception($"User with username '{username}' not found");

            await this._dbContext.Captions.AddAsync(new Caption
            {
                CreatedAt = DateTime.Now,

                Filepath = filename,
                UserId = userId.Value
            });

            await this._dbContext.SaveChangesAsync();
        }
    }
}
