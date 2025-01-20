using Media.Api.Controllers;
using Media.Api.Models;
using Media.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Media.Tests.Controllers
{
    public class MediaControllerTests
    {
        private readonly Mock<IMediaService> _mediaService;
        private readonly MediaController _mediaController;

        public MediaControllerTests()
        {
            _mediaService = new Mock<IMediaService>();
            _mediaController = new MediaController(_mediaService.Object);
        }

        [Fact]
        public async Task GetMediaItems_ShouldReturnOkMediaItems()
        {
            // Arrange
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 1, Title = "Title 1", Description = "Description 1" },
                new MediaItem { Id = 2, Title = "Title 2", Description = "Description 2" }
            };
            _mediaService
                .Setup(x => x.GetMediaItemsAsync())
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItems();

            // Assert
            var getResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(getResult.Value, mediaItems);
        }

        [Fact]
        public async Task GetMediaItems_WithEmptyList_ShouldReturnNotFound()
        {
            // Arrange
            var mediaItems = new List<MediaItem> { };
            _mediaService
                .Setup(x => x.GetMediaItemsAsync())
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItems();

            // Assert
            var getResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, getResult.StatusCode);
        }

        [Fact]
        public async Task GetMediaItems_WhenArgumentExceptionIsThrown_ShouldReturnBadRequest()
        {
            // Arrange
            _mediaService
                .Setup(x => x.GetMediaItemsAsync())
                .ThrowsAsync(new ArgumentException("ArgumentException"));

            // Act
            var result = await _mediaController.GetMediaItems();

            // Assert
            var getResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, getResult.StatusCode);
            Assert.Equal("ArgumentException", getResult.Value);
        }

        [Fact]
        public async Task GetMediaItems_WhenExceptionIsThrown_ShouldReturnStatusCode500()
        {
            // Arrange
            _mediaService
                .Setup(x => x.GetMediaItemsAsync())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _mediaController.GetMediaItems();

            // Assert
            var getResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, getResult.StatusCode);
            Assert.Equal("Exception", getResult.Value);
        }

        [Fact]
        public async Task GetMediaItem_WithValidId_ShouldReturnOkMediaItem()
        {
            // Arrange
            int id = 1;
            var mediaItem = new MediaItem { Id = id, Title = "Test" };

            _mediaService
                .Setup(x => x.GetMediaItemByIdAsync(id))
                .ReturnsAsync(mediaItem);

            // Act
            var result = await _mediaController.GetMediaItem(id);

            // Assert
            var getResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, getResult.StatusCode);
            Assert.Equal(mediaItem, getResult.Value);
        }

        [Fact]
        public async Task GetMediaItem_WitInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int id = 1;
            var mediaItem = new MediaItem { Id = id, Title = "Test" };

            _mediaService
                .Setup(x => x.GetMediaItemByIdAsync(id))
                .ReturnsAsync(mediaItem);

            // Act
            var result = await _mediaController.GetMediaItem(2);

            // Assert
            var getResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, getResult.StatusCode);
        }

        [Fact]
        public async Task GetMediaItemsWithTitle_WithValidTitle_ShouldReturnOkMediaItem()
        {
            // Arrange
            string title = "testTitle";
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 0, Title = title },
                new MediaItem { Id = 1, Title = title }
            };

            _mediaService
                .Setup(x => x.GetMediaItemsWithTitleAsync(title))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItemsWithTitle(title);

            // Assert
            var getResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, getResult.StatusCode);
            Assert.Equal(getResult.Value, mediaItems);
        }

        [Fact]
        public async Task GetMediaItemsWithTitle_WithInvalidTitle_ShouldReturnNotFound()
        {
            // Arrange
            string title = "testTitle";
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 0, Title = title },
                new MediaItem { Id = 1, Title = title }
            };

            _mediaService
                .Setup(x => x.GetMediaItemsWithTitleAsync(title))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItemsWithTitle("notTestTitle");

            // Assert
            var getResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, getResult.StatusCode);
        }

        [Fact]
        public async Task GetMediaItemsByMediaType_WithValidMediaType_ShouldReturnOkMediaItem()
        {
            // Arrange
            int mediaTypeId = 1;
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 0, Title = "T1", MediaTypeId = mediaTypeId },
                new MediaItem { Id = 1, Title = "T2", MediaTypeId = mediaTypeId }
            };

            _mediaService
                .Setup(x => x.GetMediaItemsByMediaTypeAsync(mediaTypeId))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItemsByMediaType(mediaTypeId);

            // Assert
            var getResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, getResult.StatusCode);
            Assert.Equal(getResult.Value, mediaItems);
        }

        [Fact]
        public async Task GetMediaItemsByMediaType_WithInvalidMediaType_ShouldReturnNotFound()
        {
            // Arrange
            int mediaTypeId = 1;
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = 0, Title = "T1", MediaTypeId = mediaTypeId },
                new MediaItem { Id = 1, Title = "T2", MediaTypeId = mediaTypeId }
            };

            _mediaService
                .Setup(x => x.GetMediaItemsByMediaTypeAsync(mediaTypeId))
                .ReturnsAsync(mediaItems);

            // Act
            var result = await _mediaController.GetMediaItemsByMediaType(2);

            // Assert
            var getResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, getResult.StatusCode);
        }
    }
}
