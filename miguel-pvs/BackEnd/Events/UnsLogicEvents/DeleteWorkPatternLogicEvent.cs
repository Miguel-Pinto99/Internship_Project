using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class DeleteWorkPatternLogicEvent : INotification
    {
        public WorkPattern WorkPattern { get; set; }
        public DeleteWorkPatternLogicEvent (WorkPattern workPattern)
        {
            WorkPattern = workPattern;
        }
    }
}
