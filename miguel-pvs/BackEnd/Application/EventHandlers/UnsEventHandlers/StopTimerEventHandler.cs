using MediatR;
using Project1.Events.UnsEvents;
using Project1.Models;
using Project1.Timers;

namespace Project1.Application.EventHandlers.UnsEventHandlers
{
    public class StopTimerEventHandler : INotificationHandler<StopTimerEvent>
    {
        private readonly ITimerService _timerService;
        public StopTimerEventHandler(ITimerService timerService)
        {
            _timerService = timerService;
        }
        public async Task Handle(StopTimerEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser applicationUser = notification.ApplicationUser;
            await _timerService.ReinitializeTimersAsync(applicationUser, cancellationToken);
        }
    }
}
