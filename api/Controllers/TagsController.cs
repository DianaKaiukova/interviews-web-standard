// This file manages tags.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace api.Controllers
{
    // This controller handles CRUD operations for Tags in the application.
    [ApiController]
    [Route("api/tags")]

    // The TagsController class provides endpoints to manage tags.
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TagsController> _logger;

        // Constructor that accepts the database context and logger.
        public TagsController(AppDbContext context, ILogger<TagsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            try
            {
                return await _context.Tags.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tags");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tags/5/tasks
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<api.Models.Task>>> GetTagTasks(int id)
        {
            try
            {
                var tag = await _context.Tags
                    .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Task)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tag == null) return NotFound();

                return tag.TaskTags
                    .Where(tt => tt.Task != null)
                    .Select(tt => tt.Task!)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting tasks for tag {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tags
        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag([FromBody] TagDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create a new tag from the provided DTO and set a color if provided.
            var tag = new Tag
            {
                Name = dto.Name ?? string.Empty,
                Color = dto.Color
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, tag);
        }

        // PUT: api/tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] TagDto dto)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            tag.Name = dto.Name ?? string.Empty;
            tag.Color = dto.Color;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null) return NotFound();

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting tag {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Helper method to check if a tag exists by ID.
        private bool TagExists(int id) =>
            _context.Tags.Any(e => e.Id == id);
    }
}

