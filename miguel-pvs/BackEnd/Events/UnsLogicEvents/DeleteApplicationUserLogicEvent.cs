using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class DeleteApplicationUserLogicEvent : INotification
    {
        public ApplicationUser ApplicationUser { get; set; }
        public UsersEachLocation UsersEachLocation { get; set; }
        public DeleteApplicationUserLogicEvent (ApplicationUser applicationUser, UsersEachLocation usersEachLocation)
        {
            ApplicationUser = applicationUser;
            UsersEachLocation = usersEachLocation;
        }
    }
}
