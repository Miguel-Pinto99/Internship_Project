using System.Text.Json.Serialization;

namespace Project1.Models
{
    public class WorkPattern
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<WorkPatternPart>? Parts { get; set; }
    }
}



