using MediatR;
using Project1.Application.ApplicationUsers.Queries.GetApplicationUser;
using Project1.Application.Absent.Commands.CreateAbsent;
using Project1.Application.Absent.Commands.DeleteAbsent;
using Project1.Events;
using Project1.Events.UnsEvents;
using Project1.Infrastructure;
using Project1.Models;
using Project1.Persistance;
using Project1.Events.AbsentLogicEvents;

namespace Project1.Application.Absent.Commands.DeleteAbsent
{
    public class DeleteAbsentHandler : IRequestHandler<DeleteAbsentCommand, DeleteAbsentResponse>
    {
        private readonly IAbsentRepository _repository;
        private readonly IMediator _mediator;

        public DeleteAbsentHandler(IAbsentRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;

        }

        public async Task<DeleteAbsentResponse> Handle(DeleteAbsentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = command.Id;
            var deletedAbsent = await _repository.DeleteAbsentAsync(id, cancellationToken);

            var eventPublishAbsent = new AbsentLogicEvent(deletedAbsent);
            await _mediator.Publish(eventPublishAbsent, cancellationToken);


            return new DeleteAbsentResponse
            {
                Absent = deletedAbsent
            };
        }
    }
}
