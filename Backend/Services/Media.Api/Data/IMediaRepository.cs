using Media.Api.Models;
using Shared.DataAccess;

namespace Media.Api.Data
{
    public interface IMediaRepository : IGenericRepository<MediaItem>
    {
        Task<IEnumerable<MediaItem>> GetByMediaTypeIdAsync(int mediaTypeId);
        Task<IEnumerable<MediaItem>> GetByTitleAsync(string title);
    }
}
