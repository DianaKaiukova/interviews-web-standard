using System.Text.Json.Serialization;

namespace api.Models
{
    // This class represents a many-to-many relationship between Task and Tag entities.
    public class TaskTag
    {
        // Unique identifier for the TaskTag relationship.
        public int TaskId { get; set; }
        public int TagId { get; set; }

        // Navigation properties to link to the Task and Tag entities.
        // These properties are ignored during JSON serialization to avoid circular references.
        [JsonIgnore]
        public Task? Task { get; set; }

        [JsonIgnore]
        public Tag? Tag { get; set; }
    }
}