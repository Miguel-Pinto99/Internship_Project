using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.ApplicationUsers.Queries.GetAllApplicationUser
{
    public class GetAllApplicationUserHandler : IRequestHandler<GetAllApplicationUserCommand, GetAllApplicationUserResponse>
    {
        private readonly IApplicationUsersRepository _repository;

        public GetAllApplicationUserHandler(IApplicationUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAllApplicationUserResponse> Handle(GetAllApplicationUserCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var gotUsers = await _repository.GetAllApplicationUserAsync(cancellationToken);

            return new GetAllApplicationUserResponse
            {
                listApplicationUser = gotUsers
            };
        }
    }
}
