using MediatR;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;

namespace Project1.Application.Uns.EventHandlers
{
    public class PublishWorkPatternEventHandler : INotificationHandler<PublishWorkPatternEvent>
    {
            private readonly IUnsService _unsService;

            public PublishWorkPatternEventHandler(IUnsService unsService)
            {
                _unsService = unsService;
            }

            public async Task Handle(PublishWorkPatternEvent notification, CancellationToken cancellationToken)
            {
                if (notification is null)
                {
                    throw new ArgumentNullException(nameof(notification));
                }

                Models.ApplicationUser applicationUser = notification.ApplicationUser;
                await _unsService.CallListWorkPatternAsync(applicationUser, cancellationToken);
            }
    }
}
