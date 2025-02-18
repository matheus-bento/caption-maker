using CaptionMaker.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CaptionMaker.Data
{
    public class CaptionContext : DbContext
    {
        public CaptionContext(DbContextOptions<CaptionContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Caption> Captions { get; set; }
    }
}
