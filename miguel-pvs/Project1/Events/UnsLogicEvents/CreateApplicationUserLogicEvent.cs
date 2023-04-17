using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class CreateApplicationUserLogicEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public CreateApplicationUserLogicEvent (ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
