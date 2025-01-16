using Media.Api.Models;
using Shared.DataAccess;

namespace Media.Api.Data
{
    public class MediaTypeRepository : GenericRepository<MediaType>, IMediaTypeRepository
    {
        public MediaTypeRepository(MediaDbContext context) : base(context)
        {
        }
    }
}
