using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    // This class represents a Data Transfer Object (DTO) for a Tag.
    public class TagDto
    {
        // The Name property is required and represents the name of the tag.
        // It is used to transfer data between the client and server.
        [Required]
        public string? Name { get; set; }

        public string? Color { get; set; } = "blue";
    }
}
