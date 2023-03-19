using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.WorkPatterns.Queries.GetWorkPattern
{
    public class GetWorkPatternHandler : IRequestHandler<GetWorkPatternCommand, GetWorkPatternResponse>
    {
        private readonly IWorkPatternRepository _repository;

        public GetWorkPatternHandler(IWorkPatternRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetWorkPatternResponse> Handle(GetWorkPatternCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = command.Id;
            var gotWP = await _repository.GetWorkPatternAsync(id, cancellationToken);

            return new GetWorkPatternResponse
            {
                WorkPattern = gotWP
            };
        }
    }
}
