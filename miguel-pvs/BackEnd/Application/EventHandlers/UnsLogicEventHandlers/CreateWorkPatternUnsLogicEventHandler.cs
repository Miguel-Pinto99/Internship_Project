using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Models;
using Project1.Timers;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class CreateWorkPatternUnsLogicEventHandler : INotificationHandler<CreateWorkPatternLogicEvent>
    {
        private readonly IMediator _mediator;
        public CreateWorkPatternUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(CreateWorkPatternLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            WorkPattern createdWorkPattern = notification.WorkPattern;

            var commandGetApplicationUser = new GetApplicationUserCommand(createdWorkPattern.UserId);
            var gotApplicationUser = await _mediator.Send(commandGetApplicationUser, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            var eventStopTimer = new StopTimerEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventStopTimer, cancellationToken);
        }      

    }
}
