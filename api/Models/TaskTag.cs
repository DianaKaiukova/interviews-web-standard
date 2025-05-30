using System.Text.Json.Serialization;

namespace api.Models
{
    // This class represents a many-to-many relationship between Task and Tag entities.
    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }

        [JsonIgnore]
        public Task? Task { get; set; }

        [JsonIgnore]
        public Tag? Tag { get; set; }
    }
}