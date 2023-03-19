using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;
using Project1.Events;
using Project1.Events.UnsEvents;
using Project1.Events.UnsLogicEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.WorkPatterns.Commands.DeleteWorkPattern
{
    public class DeleteWorkPatternHandler : IRequestHandler<DeleteWorkPatternCommand, DeleteWorkPatternResponse>
    {
        private readonly IWorkPatternRepository _repository;
        private readonly IMediator _mediator;

        public DeleteWorkPatternHandler(IWorkPatternRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;

        }

        public async Task<DeleteWorkPatternResponse> Handle(DeleteWorkPatternCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = command.Id;
            var deletedWorkPattern = await _repository.DeleteWorkPatternAsync(id, cancellationToken);
            var eventPublishWorkPattern = new DeleteWorkPatternLogicEvent(deletedWorkPattern);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);


            return new DeleteWorkPatternResponse
            {
                WorkPattern = deletedWorkPattern
            };
        }
    }
}
