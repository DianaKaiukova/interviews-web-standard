using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TagDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
