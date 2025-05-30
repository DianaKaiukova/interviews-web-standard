// This file contains the launch settings for the ASP.NET Core application.
// It defines how the application should be launched, including URLs and environment variables.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Controllers;
using api.Data;
using api.Models;
using Xunit;

namespace api.Tests.Controllers
{
    // This class contains unit tests for the TagsController.
    public class TagsControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly TagsController _controller;
        private readonly ILogger<TagsController> _logger;

        // This constructor initializes the in-memory database and the controller for testing.
        public TagsControllerTests()
        {
            // Use in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            // Initialize logger
            _logger = new Logger<TagsController>(new LoggerFactory());

            _controller = new TagsController(_context, _logger);
        }

        // This method is called to clean up resources after tests are done.
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        // This test verifies that the GetTags method returns all tags from the database.
        [Fact]
        public async System.Threading.Tasks.Task GetTags_ReturnsAllTags()
        {
            // Arrange
            _context.Tags.AddRange(
                new Tag { Name = "Tag 1" },
                new Tag { Name = "Tag 2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTags();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Tag>>>(result);
            var returnValue = Assert.IsType<List<Tag>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // This test verifies that the GetTags method returns an empty list when no tags exist.
        [Fact]
        public async System.Threading.Tasks.Task CreateTag_ReturnsCreatedTag()
        {

            // Act
            var newTagDto = new TagDto { Name = "New Tag" };
            var result = await _controller.CreateTag(newTagDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Tag>(actionResult.Value);
            Assert.Equal(newTagDto.Name, returnValue.Name);
            Assert.True(returnValue.Id > 0);
        }

        // This test verifies that the CreateTag method returns BadRequest when the model state is invalid.
        [Fact]
        public async System.Threading.Tasks.Task GetTagTasks_ReturnsTasksForTag()
        {
            // Arrange
            var tag = new Tag { Name = "Test Tag" };
            var task1 = new api.Models.Task { Title = "Task 1" };
            var task2 = new api.Models.Task { Title = "Task 2" };

            _context.Tags.Add(tag);
            _context.Tasks.AddRange(task1, task2);
            _context.TaskTags.AddRange(
                new TaskTag { Task = task1, Tag = tag },
                new TaskTag { Task = task2, Tag = tag }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTagTasks(tag.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<api.Models.Task>>>(result);
            var returnValue = Assert.IsType<List<api.Models.Task>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}