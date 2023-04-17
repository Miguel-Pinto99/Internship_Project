using MediatR;
using Project1.Events.AbsentLogicEvents;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;

namespace Project1.Events.AbsentLogicEvents
{
    public class AbsentLogicEvent : INotification
    {
        public Absent Absent { get; set; }
        public AbsentLogicEvent(Absent absent)
        {
            Absent = absent;
        }
    }
}