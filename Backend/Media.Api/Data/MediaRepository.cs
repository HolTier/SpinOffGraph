using MediaAPI.Models;
using Shared.DataAccess;

namespace MediaAPI.Data
{
    public class MediaRepository : GenericRepository<Media>, IMediaRepository
    {
        public MediaRepository(MediaDbContext context) : base(context)
        {
        }
    }
}
