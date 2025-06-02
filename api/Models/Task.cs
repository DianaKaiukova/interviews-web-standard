using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api.Models
{
    // This class represents a Task entity in the application.
    public class Task
    {
        // Unique identifier for the task.
        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        
        // The title of the task, required and limited to 100 characters.
        public string Title { get; set; } = string.Empty;


        // The description of the task, optional and limited to 500 characters.
        [StringLength(500)]
        public string? Description { get; set; }

        // The due date of the task, optional.
        [JsonIgnore]
        public List<TaskTag> TaskTags { get; set; } = new();
    }
}