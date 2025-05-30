using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TaskDto
    {
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
