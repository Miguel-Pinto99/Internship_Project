using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class RemoveTimerEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public RemoveTimerEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
