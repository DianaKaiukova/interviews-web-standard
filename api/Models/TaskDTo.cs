using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    // This class represents a Data Transfer Object (DTO) for a Tag.
    public class TaskDto
    {
        // The Id property is optional and can be null.
        [Required]
        public string? Title { get; set; }

        // The Description property is optional and can be null.
        public string? Description { get; set; }
    }
}
