using Media.Api.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess;

namespace Media.Api.Data
{
    public class MediaRepository : GenericRepository<MediaItem>, IMediaRepository
    {
        public MediaRepository(MediaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MediaItem>> GetByMediaTypeIdAsync(int mediaTypeId)
        {
            var mediaDbContext = (MediaDbContext)_context;
            return await mediaDbContext.MediaItems.Where(m => m.MediaTypeId == mediaTypeId).ToListAsync();
        }
    }
}
