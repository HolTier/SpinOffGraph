using Media.Api.Models;
using Shared.DataAccess;

namespace Media.Api.Data
{
    public class MediaRepository : GenericRepository<MediaItem>, IMediaRepository
    {
        public MediaRepository(MediaDbContext context) : base(context)
        {
        }
    }
}
