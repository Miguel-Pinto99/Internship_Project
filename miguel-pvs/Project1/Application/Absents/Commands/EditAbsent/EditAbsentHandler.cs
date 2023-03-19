using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.Absent.Commands.EditAbsent;
using Project1.Events;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;
using Project1.Events.AbsentLogicEvents;

namespace Project1.Application.Absent.Commands.EditAbsent
{
    public class EditAbsentHandler : IRequestHandler<EditAbsentCommand, EditAbsentResponse>
    {
        private readonly IAbsentRepository _repository;
        private readonly IMediator _mediator;

        public EditAbsentHandler(IAbsentRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<EditAbsentResponse> Handle(EditAbsentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var absent = new Models.Absent
            {
                Id = command.Id,
                StartDate = command.Body.StartDate,
                EndDate = command.Body.EndDate,
                Description = command.Body?.Description,
            };

            var updatedAbsent = await _repository.UpdateAbsentAsync(absent, cancellationToken);
            var eventPublishAbsent = new AbsentLogicEvent(updatedAbsent);
            await _mediator.Publish(eventPublishAbsent, cancellationToken);


            return new EditAbsentResponse
            {
                Absent = updatedAbsent
            };
        }
    }
}
