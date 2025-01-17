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
    }
}
