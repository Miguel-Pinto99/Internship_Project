using MediatR;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using System.Threading;

namespace Project1.Application.EventHandlers
{
    public class DeleteTopicApplicationUserEventHandler: INotificationHandler<DeleteTopicApplicationUserEvent>
    {
        private readonly IUnsService _unsService;
        public DeleteTopicApplicationUserEventHandler(IUnsService unsService)
        {
            _unsService = unsService;
        }
        public async Task Handle(DeleteTopicApplicationUserEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            Models.ApplicationUser applicationUser = notification.ApplicationUser;
            await _unsService.DeleteTopicApplicationUserAsync(applicationUser, cancellationToken);
        }
    }
}
