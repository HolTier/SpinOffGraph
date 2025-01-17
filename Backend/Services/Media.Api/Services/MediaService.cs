using Media.Api.Data;
using Media.Api.Models;

namespace Media.Api.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaTypeRepository _mediaTypeRepository;

        public MediaService(IMediaRepository mediaRepository, IMediaTypeRepository mediaTypeRepository)
        {
            _mediaRepository = mediaRepository;
            _mediaTypeRepository = mediaTypeRepository;
        }
        
        public async Task AddMediaItemAsync(MediaItem mediaItem)
        {
            // Validate mediaItem
            if (!await ValidateMediaItemAsync(mediaItem))
            {
                throw new ArgumentException("MediaItem is not valid");
            }

            await _mediaRepository.AddAsync(mediaItem);
        }

        public async Task<MediaItem?> GetMediaItemByIdAsync(int id)
        {
            return await _mediaRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<MediaItem>> GetMediaItemsAsync()
        {
            return _mediaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<MediaItem>> GetMediaItemsByMediaTypeAsync(int mediaTypeId)
        {
            // Validate MediaType
            if(!await ValidateMediaItemByMediaTypeAsync(mediaTypeId))
            {
                throw new ArgumentException("MediaType is not valid");
            }

            return await _mediaRepository.GetByMediaTypeIdAsync(mediaTypeId);
        }

        public async Task<IEnumerable<MediaItem>> GetMediaItemsWithTitleAsync(string title)
        {
            // Validate title
            if (!await ValidateMediaItemByTitleAsync(title))
            {
                throw new ArgumentException("Title is not valid");
            }

            return await _mediaRepository.GetByTitleAsync(title);
        }

        public async Task RemoveMediaItemAsync(MediaItem mediaItem)
        {
            // Validate mediaItem
            if (!await ValidateMediaItemAsync(mediaItem))
            {
                throw new ArgumentException("MediaItem is not valid");
            }

            await _mediaRepository.RemoveAsync(mediaItem);
        }

        public async Task UpdateMediaItemAsync(MediaItem mediaItem)
        {
            // Validate mediaItem
            if (!await ValidateMediaItemAsync(mediaItem))
            {
                throw new ArgumentException("MediaItem is not valid");
            }

            await _mediaRepository.UpdateAsync(mediaItem);
        }

        public async Task<bool> ValidateMediaItemAsync(MediaItem mediaItem)
        {
            // Validate all properties of mediaItem
            return await Task.FromResult(
                await ValidateMediaItemByTitleAsync(mediaItem.Title) &&
                await ValidateMediaItemByGenreAsync(mediaItem.Genre) &&
                await ValidateMediaItemByMediaTypeAsync(mediaItem.MediaTypeId) &&
                await ValidateMediaItemByImageUrlAsync(mediaItem.ImageUrl) &&
                await ValidateMediaItemByDescriptionAsync(mediaItem.Description)
            );
        }

        public async Task<bool> ValidateMediaItemByDescriptionAsync(string description)
        {
            // Description should be at least 2 characters long and at most 1000 characters long
            return await Task.FromResult(description.Length >= 2 && description.Length <= 1000);
        }

        public async Task<bool> ValidateMediaItemByGenreAsync(string genre)
        {
            // Genre should be at least 2 characters long and at most 100 characters long
            return await Task.FromResult(genre.Length >= 2 && genre.Length <= 100);
        }

        public async Task<bool> ValidateMediaItemByImageUrlAsync(string imageUrl)
        {
            // ImageUrl should be at least 2 characters long and at most 100 characters long, also it should be a valid URL
            return await Task.FromResult(imageUrl.Length >= 2 && imageUrl.Length <= 100 && Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute));
        }

        public async Task<bool> ValidateMediaItemByMediaTypeAsync(int mediaTypeId)
        {
            // MediaTypeId should be at in database
            var mediaType = await _mediaTypeRepository.GetByIdAsync(mediaTypeId);
            return mediaType != null;
        }

        public async Task<bool> ValidateMediaItemByTitleAsync(string title)
        {
            // Title should be at least 2 characters long and at most 100 characters long
            return await Task.FromResult(!String.IsNullOrEmpty(title) && title.Length >= 2 && title.Length <= 100);
        }
    }
}
