using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class UpdateTimerEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public UpdateTimerEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
