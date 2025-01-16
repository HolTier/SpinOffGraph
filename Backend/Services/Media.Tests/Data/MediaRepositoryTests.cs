using Media.Api.Data;
using Media.Api.Models;
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
    [Collection("Database collection")]
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

        [Fact]
        public async Task MediaGetAllAsync_ShouldReturnAllItems()
        {
            //Arrange
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IMediaRepository>();

            var item1 = new MediaItem
            {
                Id = 1,
                Title = "Test Title 1",
                Genre = "Test Genre 1",
                ImageUrl = "Test ImageUrl 1",
                Description = "Test Description 1",
                MediaTypeId = 1
            };

            var item2 = new MediaItem
            {
                Id = 2,
                Title = "Test Title 2",
                Genre = "Test Genre 2",
                ImageUrl = "Test ImageUrl 2",
                Description = "Test Description 2",
                MediaTypeId = 2
            };

            //Act
            await repository.AddAsync(item1);
            await repository.AddAsync(item2);

            var result = await repository.GetAllAsync();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Title == item1.Title);
            Assert.Contains(result, x => x.Title == item2.Title);
        }

        [Fact]
        public async Task MediaUpdate_WithValidItem_ShouldUpdateItemInDatabase()
        {
            //Arrange
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
            await repository.AddAsync(item);

            //Act
            item.Title = "Updated Title";
            await repository.UpdateAsync(item);
            var result = await repository.GetByIdAsync(item.Id);

            //Assert
            Assert.Equal(item.Title, result.Title);
        }

        [Fact]
        public async Task MediaUpdateAsync_WithInvalidItem_ShouldThrowException()
        {
            //Arrange
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

            //Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateAsync(item));
        }

        [Fact]
        public async Task MediaRemoveAsync_WithValidItem_ShouldRemoveItemFromDatabase()
        {
            //Arrange
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
            await repository.AddAsync(item);

            //Act
            await repository.RemoveAsync(item);
            var result = await repository.GetAllAsync();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task MediaRemoveAsync_WithInvalidItem_ShouldThrowException()
        {
            //Arrange
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

            //Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.RemoveAsync(item));
        }
    }
}
