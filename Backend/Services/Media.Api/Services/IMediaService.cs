using Media.Api.Models;

namespace Media.Api.Services
{
    public interface IMediaService
    {
        Task<IEnumerable<MediaItem>> GetMediaItemsAsync();
        Task<MediaItem?> GetMediaItemByIdAsync(int id);
        Task<IEnumerable<MediaItem>> GetMediaItemsWithTitleAsync(string title); // Should be one, but title isn't unique so it could be more
        Task<IEnumerable<MediaItem>> GetMediaItemsByMediaTypeAsync(int mediaTypeId);
        Task AddMediaItemAsync(MediaItem mediaItem);
        Task UpdateMediaItemAsync(MediaItem mediaItem);
        Task RemoveMediaItemAsync(MediaItem mediaItem);

        Task<bool> ValidateMediaItemAsync(MediaItem mediaItem);
        Task<bool> ValidateMediaItemByTitleAsync(string title);
        Task<bool> ValidateMediaItemByGenreAsync(string genre);
        Task<bool> ValidateMediaItemByMediaTypeAsync(int mediaTypeId);
        Task<bool> ValidateMediaItemByImageUrlAsync(string imageUrl);
        Task<bool> ValidateMediaItemByDescriptionAsync(string description);
    }
}
