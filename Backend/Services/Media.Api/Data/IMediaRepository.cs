using MediaAPI.Models;
using Shared.DataAccess;

namespace MediaAPI.Data
{
    public interface IMediaRepository : IGenericRepository<MediaItem>
    {
    }
}
