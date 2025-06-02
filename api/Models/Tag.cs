using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace api.Models
{
    // This class represents a Tag entity in the application.
    public class Tag
    {
        // Unique identifier for the tag.
        [Key]
        public int Id { get; set; }

        // Name of the tag, required and limited to 50 characters.
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // Color property to represent the tag's color, defaulting to "blue".
        public string? Color { get; set; } = "blue";

        // Navigation property to link tags to tasks through TaskTag.
        // This property is ignored during JSON serialization to avoid circular references.
        [JsonIgnore]
        public List<TaskTag> TaskTags { get; set; } = new();
    }
}