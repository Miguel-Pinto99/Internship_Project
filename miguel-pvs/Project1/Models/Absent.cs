using System.Text.Json.Serialization;


namespace Project1.Models
{
    public class Absent
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public int UserId { get; set; }      
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}



