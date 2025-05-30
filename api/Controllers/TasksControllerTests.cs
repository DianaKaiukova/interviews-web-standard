// This file is dedicated for testing the TasksController.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Controllers;
using api.Data;
using api.Models;
using Xunit;

namespace api.Tests.Controllers
{
    
    // This class contains unit tests for the TasksController.
    public class TasksControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly TasksController _controller;
        private readonly ILogger<TasksController> _logger;

        // This constructor initializes the in-memory database and the controller for testing.
        public TasksControllerTests()
        {
            // Use in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            // Initialize logger (using NullLogger for testing)
            _logger = new Logger<TasksController>(new LoggerFactory());

            _controller = new TasksController(_context, _logger);
        }

        // This method is called to clean up resources after tests are done.
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // This test verifies that the GetTasks method returns all tasks from the database.
        [Fact]
        public async System.Threading.Tasks.Task GetTasks_ReturnsAllTasks()
        {
            // Arrange
            _context.Tasks.AddRange(
                new api.Models.Task { Title = "Task 1" },
                new api.Models.Task { Title = "Task 2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<api.Models.Task>>>(result);
            var returnValue = Assert.IsType<List<api.Models.Task>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // This test verifies that the GetTask method returns a specific task by ID.
        [Fact]
        public async System.Threading.Tasks.Task GetTask_ReturnsTask_WhenExists()
        {
            // Arrange
            var task = new api.Models.Task { Title = "Test Task" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTask(task.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<api.Models.Task>>(result);
            var returnValue = Assert.IsType<api.Models.Task>(actionResult.Value);
            Assert.Equal(task.Id, returnValue.Id);
        }

        // This test verifies that the GetTask method returns NotFound when the task does not exist.
        [Fact]
        public async System.Threading.Tasks.Task GetTask_ReturnsNotFound_WhenNotExists()
        {
            // Act
            var result = await _controller.GetTask(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // This test verifies that the CreateTask method returns a created task.
        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ReturnsCreatedTask()
        {
            // Arrange
            var newTaskDto = new api.Models.TaskDto { Title = "New Task" };

            // Act
            var result = await _controller.CreateTask(newTaskDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<api.Models.Task>(actionResult.Value);
            Assert.Equal(newTaskDto.Title, returnValue.Title);
            Assert.True(returnValue.Id > 0);
        }

        // This test verifies that the CreateTask method returns BadRequest when the model state is invalid.
        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var task = new api.Models.Task { Title = "Original Title" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated Title";
            var taskDto = new api.Models.TaskDto { Title = task.Title };

            // Act
            var result = await _controller.UpdateTask(task.Id, taskDto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var updatedTask = await _context.Tasks.FindAsync(task.Id);
            Assert.Equal("Updated Title", updatedTask?.Title);
        }

        // This test verifies that the UpdateTask method returns NotFound when the task does not exist.
        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var task = new api.Models.Task { Title = "Task to delete" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteTask(task.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var deletedTask = await _context.Tasks.FindAsync(task.Id);
            Assert.Null(deletedTask);
        }

        // This test verifies that the DeleteTask method returns NotFound when the task does not exist.
        [Fact]
        public async System.Threading.Tasks.Task AddTagsToTask_AddsRelationships()
        {
            // Arrange
            var task = new api.Models.Task { Title = "Task with tags" };
            var tag1 = new Tag { Name = "Tag 1" };
            var tag2 = new Tag { Name = "Tag 2" };

            _context.Tasks.Add(task);
            _context.Tags.AddRange(tag1, tag2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.AddTagsToTask(task.Id, new List<int> { tag1.Id, tag2.Id });

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify relationships
            var taskTags = await _context.TaskTags
                .Where(tt => tt.TaskId == task.Id)
                .ToListAsync();

            Assert.Equal(2, taskTags.Count);
        }
    }
}