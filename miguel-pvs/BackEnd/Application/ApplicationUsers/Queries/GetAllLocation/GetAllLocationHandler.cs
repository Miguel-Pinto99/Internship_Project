using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.ApplicationUsers.Queries.GetAllLocation
{
    public class GetAllLocationHandler : IRequestHandler<GetAllLocationCommand, GetAllLocationResponse>
    {
        private readonly IApplicationUsersRepository _repository;

        public GetAllLocationHandler(IApplicationUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAllLocationResponse> Handle(GetAllLocationCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var listUsersEachLocation = await _repository.GetAllLocationsAsync(cancellationToken);

            return new GetAllLocationResponse
            {
                ListUserEachLocation = listUsersEachLocation
            };
        }
    }
}
