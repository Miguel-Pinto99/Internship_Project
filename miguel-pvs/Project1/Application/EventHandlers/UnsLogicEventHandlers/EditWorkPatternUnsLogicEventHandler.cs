using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Models;
using Project1.Timers;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class EditWorkPatternUnsLogicEventHandler : INotificationHandler<EditWorkPatternLogicEvent>
    {
        private readonly IMediator _mediator;
        public EditWorkPatternUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(EditWorkPatternLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            WorkPattern updatedWorkPattern = notification.WorkPattern;

            var commandGetApplicationUser= new GetApplicationUserCommand(updatedWorkPattern.UserId);
            var gotApplicationUser = await _mediator.Send(commandGetApplicationUser, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            var eventStopTimer = new StopTimerEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventStopTimer, cancellationToken);
        }

        


    }
}
