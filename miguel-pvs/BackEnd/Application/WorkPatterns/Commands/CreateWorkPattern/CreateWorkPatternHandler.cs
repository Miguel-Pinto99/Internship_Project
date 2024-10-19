using MediatR;
using Project1.Application.WorkPatterns.Queries.GetWorkPattern;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.WorkPatterns.Commands.CreateWorkPattern
{
    public class CreateWorkPatternHandler : IRequestHandler<CreateWorkPatternCommand, CreateWorkPatternResponse>
    {
        private readonly IWorkPatternRepository _repository;
        private readonly IMediator _mediator;

        public CreateWorkPatternHandler(IWorkPatternRepository repository,
            IMediator mediator,
            IUnsService unsService)
        {
            _repository = repository;
            _mediator = mediator;

        }

        public async Task<CreateWorkPatternResponse> Handle(CreateWorkPatternCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var workPattern = new WorkPattern
            {
                UserId = command.UserId,
                StartDate = command.Body.StartDate,
                EndDate = command.Body.EndDate,
                Parts = command.Body.Parts
            };

            var createdWorkPattern = await _repository.CreateWorkPatternAsync(workPattern, cancellationToken);
            var eventPublishWorkPattern = new CreateWorkPatternLogicEvent(createdWorkPattern);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);

            return new CreateWorkPatternResponse
            {
                WorkPattern = workPattern
            };
        }
    }
}
