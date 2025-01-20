using Media.Api.Data;
using Media.Api.Models;
using Media.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Tests.Services
{
    public class MediaServiceTests
    {
        private readonly Mock<IMediaRepository> _mediaRepositoryMock;
        private readonly Mock<IMediaTypeRepository> _mediaTypeRepositoryMock;
        private readonly MediaService _mediaService;

        public MediaServiceTests()
        {
            _mediaRepositoryMock = new Mock<IMediaRepository>();
            _mediaTypeRepositoryMock = new Mock<IMediaTypeRepository>();
            _mediaService = new MediaService(_mediaRepositoryMock.Object, _mediaTypeRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidateMediaItemAsync_WithValidItem_ShouldReturnTrue()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "Test Title",
                Genre = "Test Genre",
                ImageUrl = "http://www.test.com/test.jpg",
                Description = "Test Description",
                MediaTypeId = 1
            };

            var validMediaType = new MediaType { Id = 1, Name = "Test MediaType" };

            _mediaTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(validMediaType);

            // Act
            var result = await _mediaService.ValidateMediaItemAsync(item);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateMediaItemAsync_WithInvalidItem_ShouldReturnFalse()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "T", // Too short title
                Genre = "Test Genre",
                ImageUrl = "Test ImageUrl", // Bad url
                Description = "Test Description",
                MediaTypeId = 1
            };

            // Act
            var result = await _mediaService.ValidateMediaItemAsync(item);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByTitleAsync_WithValidTitle_ShouldReturnTrue()
        {
            // Arrange
            var title = "Test Title";
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = title },
                new MediaItem { Id = 2, Title = title },
                new MediaItem { Id = 3, Title = title }
            };

            _mediaRepositoryMock
                .Setup(x => x.GetByTitleAsync(title))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaService.ValidateMediaItemByTitleAsync(title);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("T")] // Too short title
        [InlineData("")] // Empty title
        [InlineData(null)] // Null title
        public async Task ValidateMediaItemByTitleAsync_WithInvalidTitle_ShouldReturnFalse(string title)
        {
            // Act
            var result = await _mediaService.ValidateMediaItemByTitleAsync(title);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByTitleAsync_WithTooLongTitle_ShouldReturnFalse()
        {
            // Arrange
            var title = new string('T', 1001); // Too long title

            // Act
            var result = await _mediaService.ValidateMediaItemByTitleAsync(title);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByMediaTypeAsync_WithValidMediaItem_ShouldReturnTrue()
        {
            // Arrange
            var mediaType = new MediaType { Id = 1, Name = "Test MediaType" };

            _mediaTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(mediaType);

            // Act
            var result = await _mediaService.ValidateMediaItemByMediaTypeAsync(mediaType.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateMediaItemByMediaTypeAsync_WithInvalidMediaItem_ShouldReturnFalse()
        {
            // Act
            var result = await _mediaService.ValidateMediaItemByMediaTypeAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByImageUrlAsync_WithValidImageUrl_ShouldReturnTrue()
        {
            // Arrange
            var imageUrl = "http://www.test.com/test.jpg";

            // Act
            var result = await _mediaService.ValidateMediaItemByImageUrlAsync(imageUrl);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")] // Empty string
        [InlineData(null)] // Null string
        [InlineData("T")] // Too short string
        [InlineData("Valid_Url test.com")] // Invalid URL
        public async Task ValidateMediaItemByImageUrlAsync_WithInvalidImageUrl_ShouldReturnFalse(string imageUrl)
        {
            // Act
            var result = await _mediaService.ValidateMediaItemByImageUrlAsync(imageUrl);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByImageUrlAsync_WithTooLongImageUrl_ShouldReturnFalse()
        {
            // Arrange
            var imageUrl = new string('T', 1001); // Too long string

            // Act
            var result = await _mediaService.ValidateMediaItemByImageUrlAsync(imageUrl);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByDescriptionAsync_WithValidDescription_ShouldReturnTrue()
        {
            // Arrange
            var description = new string('T', 2); // Minimum length

            // Act
            var result = await _mediaService.ValidateMediaItemByDescriptionAsync(description);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")] // Empty string
        [InlineData(null)] // Null string
        [InlineData("T")] // Too short string
        public async Task ValidateMediaItemByDescriptionAsync_WithInvalidDescription_ShouldReturnFalse(string description)
        {
            // Act
            var result = await _mediaService.ValidateMediaItemByDescriptionAsync(description);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByDescriptionAsync_WithTooLongDescription_ShouldReturnFalse()
        {
            // Arrange
            var description = new string('T', 1001); // Too long string

            // Act
            var result = await _mediaService.ValidateMediaItemByDescriptionAsync(description);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByGenreAsync_WithValidGenre_ShouldReturnTrue()
        {
            // Arrange
            var genre = new string('T', 2); // Minimum length

            // Act
            var result = await _mediaService.ValidateMediaItemByGenreAsync(genre);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")] // Empty string
        [InlineData(null)] // Null string
        [InlineData("T")] // Too short string
        public async Task ValidateMediaItemByGenreAsync_WithInvalidGenre_ShouldReturnFalse(string genre)
        {
            // Act
            var result = await _mediaService.ValidateMediaItemByGenreAsync(genre);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateMediaItemByGenreAsync_WithTooLongGenre_ShouldReturnFalse()
        {
            // Arrange
            var genre = new string('T', 101); // Too long string

            // Act
            var result = await _mediaService.ValidateMediaItemByGenreAsync(genre);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddMediaItemAsync_WithValidItem_ShouldAddItemToDatabase()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "Test Title",
                Genre = "Test Genre",
                ImageUrl = "http://www.test.com/test.jpg",
                Description = "Test Description",
                MediaTypeId = 1
            };

            var validMediaType = new MediaType { Id = 1, Name = "Test MediaType" };

            _mediaTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(validMediaType);

            _mediaTypeRepositoryMock
                .Setup(repo => repo.AddAsync(validMediaType))
                .Verifiable();

            // Act
            await _mediaService.AddMediaItemAsync(item);

            // Assert
            _mediaRepositoryMock.Verify(x => x.AddAsync(item), Times.Once);
            _mediaTypeRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddMediaItemAsync_WithInvalidItem_ShouldThrowArgumentException()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "T", // Too short title
                Genre = "Test Genre",
                ImageUrl = "Test ImageUrl",
                Description = "Test Description",
                MediaTypeId = 1
            };

            _mediaRepositoryMock.Setup(x => x.AddAsync(item)).ThrowsAsync(new ArgumentException());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mediaService.AddMediaItemAsync(item));
        }

        [Fact]
        public async Task GetMediaItemByIdAsync_WithValidId_ShouldReturnItem()
        {
            // Arrange
            var id = 1;
            var item = new MediaItem { Id = id, Title = "Test Title" };

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(item);

            // Act
            var result = await _mediaService.GetMediaItemByIdAsync(id);

            // Assert
            Assert.Equal(item, result);
        }

        [Fact]
        public async Task GetMediaItemByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var item = new MediaItem { Id = id, Title = "Test Title" };

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(item);

            // Act
            var result = await _mediaService.GetMediaItemByIdAsync(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMediaItemsAsync_ShouldReturnItems()
        {
            // Arrange
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = "Test Title 1" },
                new MediaItem { Id = 2, Title = "Test Title 2" },
                new MediaItem { Id = 3, Title = "Test Title 3" }
            };

            _mediaRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaService.GetMediaItemsAsync();

            // Assert
            Assert.Equal(mediaItems, result);
        }

        [Fact]
        public async Task GetMediaItemsWithTitleAsync_WithValidTitle_ShouldReturnItems()
        {
            // Arrange
            var title = "Test Title";
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = title },
                new MediaItem { Id = 2, Title = title },
                new MediaItem { Id = 3, Title = title }
            };

            _mediaRepositoryMock
                .Setup(x => x.GetByTitleAsync(title))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaService.GetMediaItemsWithTitleAsync(title);

            // Assert
            Assert.Equal(mediaItems, result);
        }

        [Fact]
        public async Task GetMediaItemsWithTitleAsync_WithInvalidTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var title = "T"; // Too short title

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mediaService.GetMediaItemsWithTitleAsync(title));
        }

        [Fact]
        public async Task UpdateMediaItemAsync_WithValidItem_ShouldUpdateItemInDatabase()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "Test Title",
                Genre = "Test Genre",
                ImageUrl = "http://www.test.com/test.jpg",
                Description = "Test Description",
                MediaTypeId = 1
            };

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            _mediaTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new MediaType { Id = 1, Name = "Test MediaType" });

            // Act
            await _mediaService.UpdateMediaItemAsync(item);

            // Assert
            _mediaRepositoryMock.Verify(x => x.UpdateAsync(item), Times.Once);
        }

        [Fact]
        public async Task UpdateMediaItemAsync_WithInvalidItem_ShouldThrowArgumentException()
        {
            // Arrange
            var item = new MediaItem
            {
                Id = 1,
                Title = "T",
                Genre = "T",
                ImageUrl = "",
                Description = "Test Description",
                MediaTypeId = 1
            };

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mediaService.UpdateMediaItemAsync(item));
        }

        [Fact]
        public async Task GetMediaItemsByMediaTypeAsync_WithValidMediaType_ShouldReturnItems()
        {
            // Arrange
            var mediaTypeId = 1;
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = "Test Title 1", MediaTypeId = mediaTypeId },
                new MediaItem { Id = 2, Title = "Test Title 2", MediaTypeId = mediaTypeId },
                new MediaItem { Id = 3, Title = "Test Title 3", MediaTypeId = mediaTypeId }
            };

            _mediaTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(mediaTypeId))
                .ReturnsAsync(new MediaType { Id = mediaTypeId, Name = "Test MediaType" });

            _mediaRepositoryMock
                .Setup(x => x.GetByMediaTypeIdAsync(mediaTypeId))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaService.GetMediaItemsByMediaTypeAsync(mediaTypeId);

            // Assert
            Assert.Equal(mediaItems, result);
        }

        [Fact]
        public async Task RemoveMediaItemAsync_WithValidId_ShouldRemoveItemFromDatabase()
        {
            // Arrange
            var id = 1;
            var item = new MediaItem { Id = id, Title = "Test Title" };

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(item);

            // Act
            await _mediaService.RemoveMediaItemAsync(id);

            // Assert
            _mediaRepositoryMock.Verify(x => x.RemoveAsync(item), Times.Once);
        }

        [Fact]
        public async Task RemoveMediaItemAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = 1;

            _mediaRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((MediaItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _mediaService.RemoveMediaItemAsync(id));
        }
    }
}
