using Microsoft.EntityFrameworkCore;

namespace MediaAPI.Models
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) {}

        public DbSet<Media> Media { get; set; } = null!;
    }
}
