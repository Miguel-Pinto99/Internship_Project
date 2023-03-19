using MediatR;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;


namespace Project1.Application.Uns.EventHandlers
{
    public class PublishCheckInEventHandler : INotificationHandler<PublishCheckInEvent>
    {

        private readonly IUnsService _unsService;

        public PublishCheckInEventHandler(IUnsService unsService)
        {
            _unsService = unsService;
        }

        public async Task Handle(PublishCheckInEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var now = DateTime.UtcNow;
            Models.ApplicationUser applicationUser = notification.ApplicationUser;
            await _unsService.PublishCheckInAsync(applicationUser,now, cancellationToken);
        }


    }
}
