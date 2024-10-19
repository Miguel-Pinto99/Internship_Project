using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.ApplicationUsers.Queries.GetLocation
{
    public class GetLocationHandler : IRequestHandler<GetLocationCommand, GetLocationResponse>
    {
        private readonly IApplicationUsersRepository _repository;

        public GetLocationHandler(IApplicationUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetLocationResponse> Handle(GetLocationCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            int officeLocation = command.OfficeLocation;
            UsersEachLocation usersEachLocation = await _repository.GetLocationAsync(officeLocation, cancellationToken);

            return new GetLocationResponse
            {
                UserEachLocation = usersEachLocation
            };
        }
    }
}
