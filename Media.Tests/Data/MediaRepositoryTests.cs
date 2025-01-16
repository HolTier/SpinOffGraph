using MediaAPI.Data;
using MediaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Tests.Data
{
    public class MediaRepositoryTests
    {
        private readonly ServiceProvider _serviceProvider;

        public MediaRepositoryTests()
        {
            // Setup DI
            var services = new ServiceCollection();

            // Configure InMemory database
            services.AddDbContext<MediaDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            // Register repository
            services.AddScoped<IMediaRepository, MediaRepository>();

            // Build service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task MediaAddAsync_WithValidItem_ShouldAddItemToDatabase()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IMediaRepository>();

            var item = new MediaItem
            {
                Id = 1,
                Title = "Test Title",
                Genre = "Test Genre",
                ImageUrl = "Test ImageUrl",
                Description = "Test Description",
                MediaTypeId = 1

            };

            // Act
            await repository.AddAsync(item);
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Single(result);
            var itemFromDb = result.First();
            Assert.Equal(item.Id, itemFromDb.Id);
            Assert.Equal(item.Title, itemFromDb.Title);
            Assert.Equal(item.Genre, itemFromDb.Genre);
            Assert.Equal(item.ImageUrl, itemFromDb.ImageUrl);
            Assert.Equal(item.Description, itemFromDb.Description);
            Assert.Equal(item.MediaTypeId, itemFromDb.MediaTypeId);
        }
    }
}
