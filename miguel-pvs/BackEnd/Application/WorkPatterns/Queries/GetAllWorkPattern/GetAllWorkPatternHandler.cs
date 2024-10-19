using MediatR;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.WorkPatterns.Queries.GetAllWorkPattern
{
    public class GetAllWorkPatternHandler : IRequestHandler<GetAllWorkPatternCommand, GetAllWorkPatternResponse>
    {
        private readonly IWorkPatternRepository _repository;

        public GetAllWorkPatternHandler(IWorkPatternRepository repository)
        {
            _repository = repository;

        }

        public async Task<GetAllWorkPatternResponse> Handle(GetAllWorkPatternCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var gotWP = await _repository.GetAllWorkPatternsAsync(cancellationToken);

            return new GetAllWorkPatternResponse
            {
                listWorkPattern = gotWP
            };
        }
    }
}
