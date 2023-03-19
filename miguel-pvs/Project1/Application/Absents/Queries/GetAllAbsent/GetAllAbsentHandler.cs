using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.Absent.Queries.GetAllAbsent
{
    public class GetAllAbsentHandler : IRequestHandler<GetAllAbsentCommand, GetAllAbsentResponse>
    {
        private readonly IAbsentRepository _repository;

        public GetAllAbsentHandler(IAbsentRepository repository)
        {
            _repository = repository;

        }

        public async Task<GetAllAbsentResponse> Handle(GetAllAbsentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var gotAbsent = await _repository.GetAllAbsentAsync(cancellationToken);

            return new GetAllAbsentResponse
            {
                ListAbsent = gotAbsent
            };
        }
    }
}
