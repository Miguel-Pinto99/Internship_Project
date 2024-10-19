using MediatR;

using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class PublishWorkPatternEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public PublishWorkPatternEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
