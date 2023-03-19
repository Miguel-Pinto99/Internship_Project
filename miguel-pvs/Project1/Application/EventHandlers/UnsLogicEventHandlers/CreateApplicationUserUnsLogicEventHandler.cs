using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class CreateApplicationUserUnsLogicEventHandler : INotificationHandler<CreateApplicationUserLogicEvent>
    {
        private readonly IMediator _mediator;
        public CreateApplicationUserUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(CreateApplicationUserLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser createdUser = notification.ApplicationUser;

            var eventSendCheckIn = new PublishCheckInEvent(createdUser);
            await _mediator.Publish(eventSendCheckIn, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(createdUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            var commandGetOfficeLocation = new GetLocationCommand(createdUser.OfficeLocation);
            var responseGetOfficeLocation = await _mediator.Send(commandGetOfficeLocation, cancellationToken);

            var eventPublishLocation = new PublishLocationEvent(responseGetOfficeLocation.UserEachLocation, responseGetOfficeLocation.UserEachLocation.OfficeLocation);
            await _mediator.Publish(eventPublishLocation, cancellationToken);

            var eventStopTimer = new StopTimerEvent(createdUser);
            await _mediator.Publish(eventStopTimer, cancellationToken);
        }
    }
}
