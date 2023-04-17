using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class PublishCheckInEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public PublishCheckInEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }

    }
}
