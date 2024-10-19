using MediatR;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;
using System.Diagnostics;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Project1.Application.Uns.EventHandlers
{
    public class PublishLocationEventHandler : INotificationHandler<PublishLocationEvent>
    {

        private readonly IUnsService _unsService;

        public PublishLocationEventHandler(IUnsService unsService)
        {
            _unsService = unsService;
        }

        public async Task Handle(PublishLocationEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            UsersEachLocation usersEachLocation = notification.UsersEachLocation;
            int officeLocation = notification.OfficeLocation;


            await _unsService.PublishLocationAsync(usersEachLocation, officeLocation, cancellationToken);
        }


    }
}
