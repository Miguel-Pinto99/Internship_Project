using MediatR;
using Project1.Models;

namespace Project1.Application.WorkPatterns.Commands.CreateWorkPattern
{
    public class CreateWorkPatternCommandBody
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<WorkPatternPart>? Parts { get; set; }
    }
}