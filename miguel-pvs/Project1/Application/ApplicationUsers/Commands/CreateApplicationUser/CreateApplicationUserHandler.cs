using MediatR;
using Project1.Persistance;
using Project1.Events.UnsLogicEvents;

namespace Project1.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserHandler : IRequestHandler<CreateApplicationUserCommand, CreateApplicationUserResponse>
    {
        private readonly IApplicationUsersRepository _repository;
        private readonly IMediator _mediator;

        public CreateApplicationUserHandler(IApplicationUsersRepository repository, 
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<CreateApplicationUserResponse> Handle(CreateApplicationUserCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new Models.ApplicationUser
            {
                Id = command.Id,
                FirstName = command.Body.FirstName,
                OfficeLocation = command.Body.OfficeLocation
            };

            var createdUser = await _repository.CreateApplicationUserAsync(user, cancellationToken);

            var eventUnsLogic = new CreateApplicationUserLogicEvent(createdUser);
            await _mediator.Publish(eventUnsLogic, cancellationToken);

            return new CreateApplicationUserResponse
            {
                ApplicationUser = createdUser
            };
        }
    }
}
