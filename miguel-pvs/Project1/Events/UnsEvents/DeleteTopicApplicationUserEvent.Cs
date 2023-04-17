using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class DeleteTopicApplicationUserEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public DeleteTopicApplicationUserEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
