using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Events;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;

namespace Project1.Application.WorkPatterns.Commands.EditWorkPattern
{
    public class EditWorkPatternHandler : IRequestHandler<EditWorkPatternCommand, EditWorkPatternResponse>
    {
        private readonly IWorkPatternRepository _repository;
        private readonly IMediator _mediator;

        public EditWorkPatternHandler(IWorkPatternRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;

        }

        public async Task<EditWorkPatternResponse> Handle(EditWorkPatternCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var workPattern = new WorkPattern
            {
                Id = command.Id,
                StartDate = command.Body.StartDate,
                EndDate = command.Body.EndDate,
                Parts = command.Body.Parts
            };

            var updatedWP = await _repository.UpdateWorkPatternAsync(workPattern, cancellationToken);

            var commandGetApplicationUser = new GetApplicationUserCommand(updatedWP.UserId);
            var gotApplicationUser = await _mediator.Send(commandGetApplicationUser, cancellationToken);

            var eventPublishWorkPattern = new PublishWorkPatternEvent(gotApplicationUser.ApplicationUser);
            await _mediator.Publish(eventPublishWorkPattern, cancellationToken);


            return new EditWorkPatternResponse
            {
                WorkPattern = updatedWP
            };
        }
    }
}
