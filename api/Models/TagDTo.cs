using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    // This class represents a Data Transfer Object (DTO) for a Tag.
    public class TagDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
