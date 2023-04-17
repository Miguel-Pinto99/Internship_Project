using MediatR;
using Project1.Models;

namespace Project1.Events.UnsEvents
{
    public class PublishLocationEvent : INotification
    {
        public UsersEachLocation UsersEachLocation { get; set; }
        public int OfficeLocation { get; set; }
        public PublishLocationEvent(UsersEachLocation usersEachLocation, int officeLocation)
        {
            UsersEachLocation = usersEachLocation;
            OfficeLocation = officeLocation;
        }
    }
}
