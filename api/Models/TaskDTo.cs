using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    // This class represents a Data Transfer Object (DTO) for a Tag.
    public class TaskDto
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
