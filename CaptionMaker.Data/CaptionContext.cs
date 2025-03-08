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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Caption>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
