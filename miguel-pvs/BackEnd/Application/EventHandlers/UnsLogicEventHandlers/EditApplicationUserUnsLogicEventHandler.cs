using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetLocation;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;

namespace Project1.Application.EventHandlers.UnsLogicEventHandlers
{
    public class EditApplicationUserUnsLogicEventHandler : INotificationHandler<EditApplicationUserLogicEvent>
    {
        private readonly IMediator _mediator;
        public EditApplicationUserUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(EditApplicationUserLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser newUser = notification.NewApplicationUser;
            Models.ApplicationUser oldUser = notification.OldApplicationUser;
            var oldOfficeLocation = oldUser.OfficeLocation;

            if (newUser != null)
            {
                var eventDeleteOldTopic = new DeleteTopicApplicationUserEvent(oldUser);
                await _mediator.Publish(eventDeleteOldTopic, cancellationToken);

                var commandGetOldOfficeLocation = new GetLocationCommand(oldOfficeLocation);
                var responseGetOldOfficeLocation = await _mediator.Send(commandGetOldOfficeLocation, cancellationToken);

                if (responseGetOldOfficeLocation.UserEachLocation != null)
                {
                    var eventPublishOldLocation = new PublishLocationEvent(responseGetOldOfficeLocation.UserEachLocation, oldOfficeLocation);
                    await _mediator.Publish(eventPublishOldLocation, cancellationToken);
                }

                var eventDeleteNewTopic = new DeleteTopicApplicationUserEvent(newUser);
                await _mediator.Publish(eventDeleteNewTopic, cancellationToken);

                var commandGetNewOfficeLocation = new GetLocationCommand(newUser.OfficeLocation);
                var responseGetNewOfficeLocation = await _mediator.Send(commandGetNewOfficeLocation, cancellationToken);
                var eventPublishNewLocation = new PublishLocationEvent(responseGetNewOfficeLocation.UserEachLocation, responseGetNewOfficeLocation.UserEachLocation.OfficeLocation);
                await _mediator.Publish(eventPublishNewLocation, cancellationToken);

                var eventSendCheckIn = new PublishCheckInEvent(newUser);
                await _mediator.Publish(eventSendCheckIn, cancellationToken);

                var eventPublishWorkPattern = new PublishWorkPatternEvent(newUser);
                await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

                var eventStopTimer = new StopTimerEvent(newUser);
                await _mediator.Publish(eventStopTimer, cancellationToken);

            }




        }
    }
}
