using MediatR;
using Project1.Events.UnsEvents;
using Project1.Models;
using Project1.Timers;

namespace Project1.Application.Uns.UnsLogicEventHandlers
{
    public class UpdateTimerEventHandler : INotificationHandler<UpdateTimerEvent>
    {
        private readonly ITimerService _timerService;
        public UpdateTimerEventHandler(ITimerService timerService)
        {
            _timerService = timerService;
        }
        public async Task Handle(UpdateTimerEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser applicationUser = notification.ApplicationUser;
            await _timerService.CalculateDelayAsync(applicationUser,cancellationToken);
        }




    }
}