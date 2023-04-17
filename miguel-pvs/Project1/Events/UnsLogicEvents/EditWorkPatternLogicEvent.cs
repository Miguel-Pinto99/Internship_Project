using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class EditWorkPatternLogicEvent : INotification
    {
        public WorkPattern WorkPattern { get; set; }
        public EditWorkPatternLogicEvent (WorkPattern workPattern)
        {
            WorkPattern = workPattern;
        }
    }
}
