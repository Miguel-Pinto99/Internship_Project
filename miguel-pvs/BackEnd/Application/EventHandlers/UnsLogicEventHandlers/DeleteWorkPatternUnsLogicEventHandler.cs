using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Models;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class DeleteWorkPatternUnsLogicEventHandler : INotificationHandler<DeleteWorkPatternLogicEvent>
    {
        private readonly IMediator _mediator;
        public DeleteWorkPatternUnsLogicEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(DeleteWorkPatternLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            WorkPattern deletedWorkPattern = notification.WorkPattern;

            var commandGetApplicationUser = new GetApplicationUserCommand(deletedWorkPattern.UserId);
            var gotApplicationUser = await _mediator.Send(commandGetApplicationUser, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            var eventStopTimer = new StopTimerEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventStopTimer, cancellationToken);
        }

        


    }
}
