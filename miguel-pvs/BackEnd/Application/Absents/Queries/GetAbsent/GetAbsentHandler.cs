using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.Absent.Queries.GetAbsent
{
    public class GetAbsentHandler : IRequestHandler<GetAbsentCommand, GetAbsentResponse>
    {
        private readonly IAbsentRepository _repository;

        public GetAbsentHandler(IAbsentRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAbsentResponse> Handle(GetAbsentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = command.Id;
            var gotAbsent = await _repository.GetAbsentAsync(id, cancellationToken);

            return new GetAbsentResponse
            {
                Absent = gotAbsent
            };
        }
    }
}
