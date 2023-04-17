using MediatR;
using Project1.Models;

namespace Project1.Events.UnsLogicEvents
{
    public class EditApplicationUserLogicEvent : INotification
    {
        public ApplicationUser NewApplicationUser { get; set; }
        public ApplicationUser OldApplicationUser { get; set; }
        public EditApplicationUserLogicEvent (ApplicationUser newApplicationUser,ApplicationUser oldApplicationUser)
        {
            NewApplicationUser = newApplicationUser;
            OldApplicationUser = oldApplicationUser;
        }
    }
}
