using MediaAPI.Models;
using Shared.DataAccess;

namespace MediaAPI.Data
{
    public class MediaRepository : GenericRepository<MediaItem>, IMediaRepository
    {
        public MediaRepository(MediaDbContext context) : base(context)
        {
        }
    }
}
