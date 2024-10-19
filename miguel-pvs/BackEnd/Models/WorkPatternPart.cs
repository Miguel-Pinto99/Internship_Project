using System.Text.Json.Serialization;

namespace Project1.Models
{
    public class WorkPatternPart
    {
        public Guid Id { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}



