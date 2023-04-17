using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class CreateWorkPatternLogicEvent : INotification
    {
        public WorkPattern WorkPattern { get; set; }
        public CreateWorkPatternLogicEvent (WorkPattern workPattern)
        {
            WorkPattern = workPattern;
        }
    }
}
