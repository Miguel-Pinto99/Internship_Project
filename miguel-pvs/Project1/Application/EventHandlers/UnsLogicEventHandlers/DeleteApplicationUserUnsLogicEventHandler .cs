using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class DeleteApplicationUserUnsLogicEventHandler : INotificationHandler<DeleteApplicationUserLogicEvent>
    {
        private readonly IMediator _mediator;
        public DeleteApplicationUserUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(DeleteApplicationUserLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser deletedUser = notification.ApplicationUser;
            UsersEachLocation usersEachLocation = notification.UsersEachLocation;

            if (deletedUser != null)
            {

                var eventDeleteTopicApplicationUser = new DeleteTopicApplicationUserEvent(deletedUser);
                await _mediator.Publish(eventDeleteTopicApplicationUser, cancellationToken);

                if (usersEachLocation is not null)
                {
                    var officeLocation= usersEachLocation.OfficeLocation;

                    var eventPublishLocation = new PublishLocationEvent(usersEachLocation, officeLocation);
                    await _mediator.Publish(eventPublishLocation, cancellationToken);
                }

                var eventDeleteTimer = new RemoveTimerEvent(deletedUser);
                await _mediator.Publish(eventDeleteTimer, cancellationToken);
            }
        }

        


    }
}
