// This file manages tasks and their associated tags.


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    // This controller handles CRUD operations for Tasks in the application.
    [ApiController]
    [Route("api/tasks")]
    
    // The TasksController class provides endpoints to manage tasks.
    public class TasksController : ControllerBase
    {

        // This controller manages tasks and their associated tags.
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<api.Models.Task>>> GetTasks()
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<api.Models.Task>> GetTask(int id)
        {
            try
            {
                var task = await _context.Tasks
                    .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (task == null) return NotFound();
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting task {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<api.Models.Task>> CreateTask([FromBody] TaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.Title == null)
            {
                return BadRequest("Title cannot be null.");
            }

            var task = new api.Models.Task
            {
                Title = dto.Title,
                Description = dto.Description
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }


        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            if (dto.Title == null)
            {
                return BadRequest("Title cannot be null.");
            }
            task.Title = dto.Title;
            task.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null) return NotFound();

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tasks/5/tags
        [HttpPost("{taskId}/tags")]
        public async Task<IActionResult> AddTagsToTask(int taskId, [FromBody] List<int> tagIds)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null) return NotFound("Task not found");

                foreach (var tagId in tagIds)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag == null) return NotFound($"Tag {tagId} not found");

                    if (!_context.TaskTags.Any(tt => tt.TaskId == taskId && tt.TagId == tagId))
                    {
                        _context.TaskTags.Add(new TaskTag { TaskId = taskId, TagId = tagId });
                    }
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding tags to task {taskId}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/tasks/5/tags/3
        private bool TaskExists(int id) =>
            _context.Tasks.Any(e => e.Id == id);
    }
}