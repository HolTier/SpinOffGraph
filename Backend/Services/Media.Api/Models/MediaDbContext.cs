using Microsoft.EntityFrameworkCore;

namespace Media.Api.Models
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) {}

        public DbSet<MediaItem> MediaItems { get; set; } = null!;
        public DbSet<MediaType> MediaTypes { get; set; } = null!;
    }
}
