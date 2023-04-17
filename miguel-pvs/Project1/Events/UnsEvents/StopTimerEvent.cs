using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class StopTimerEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public StopTimerEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
