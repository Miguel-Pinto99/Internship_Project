using MediatR;
using Project1.Application.Absent.Queries.GetAllAbsent;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events.AbsentLogicEvents;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Timers;

namespace Project1.Application.EventHandlers.AbsentLogic
{
    public class AbsentLogicEventHandler : INotificationHandler<AbsentLogicEvent>
    {
        private readonly IUnsService _unsService;
        private readonly IMediator _mediator;
        private readonly ITimerService _timerService;

        public AbsentLogicEventHandler(IUnsService unsService
            , IMediator mediator
            , ITimerService timerService)
        {
            _unsService = unsService;
            _mediator = mediator;
            _timerService = timerService;
        }

        public async Task Handle(AbsentLogicEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.Absent absent = notification.Absent;

            var commandGetApplicationUser = new GetApplicationUserCommand(absent.UserId);
            var gotApplicationUser = await _mediator.Send(commandGetApplicationUser, cancellationToken);

            var commandGetAllAbsent = new GetAllAbsentCommand();
            var gotAllAbsentUsers = await _mediator.Send(commandGetAllAbsent, cancellationToken);

            await _unsService.CheckTodayAbsentsAsync(gotAllAbsentUsers.ListAbsent, cancellationToken);
            await _timerService.RemoveAbsentsAsync(gotAllAbsentUsers.ListAbsent, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            await _timerService.ReinitializeTimersAsync(gotApplicationUser.ApplicationUser, cancellationToken);

        }
    }
}