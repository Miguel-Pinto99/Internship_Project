using MediatR;
using Project1.Events.UnsEvents;
using Project1.Persistance;

namespace Project1.Application.ApplicationUsers.Queries.CheckOutApplicationUser
{
    public class CheckOutApplicationUserHandler : IRequestHandler<CheckOutApplicationUserCommand, CheckOutApplicationUserResponse>
    {
        private readonly IApplicationUsersRepository _repository;
        private readonly IMediator _mediator;

        public CheckOutApplicationUserHandler(IApplicationUsersRepository repository, 
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<CheckOutApplicationUserResponse> Handle(CheckOutApplicationUserCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            
            int id = command.Id;
            var checkOutUser = await _repository.CheckOutApplicationUserAsync(id, cancellationToken);

            var eventSendCheckIn = new PublishCheckInEvent(checkOutUser);
            await _mediator.Publish(eventSendCheckIn, cancellationToken);

            return new CheckOutApplicationUserResponse
            {
                ApplicationUser = checkOutUser
            };
        }
    }
}
