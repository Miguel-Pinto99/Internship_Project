using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserHandler : IRequestHandler<GetApplicationUserCommand, GetApplicationUserResponse>
    {
        private readonly IApplicationUsersRepository _repository;

        public GetApplicationUserHandler(IApplicationUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetApplicationUserResponse> Handle(GetApplicationUserCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            int id = command.Id;
            var getUser = await _repository.GetApplicationUserAsync(id, cancellationToken);

            return new GetApplicationUserResponse
            {
                ApplicationUser = getUser
            };
        }
    }
}
