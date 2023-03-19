using MediatR;
using Project1.Infrastructure;
using Project1.Persistance;
using Project1.Events.AbsentLogicEvents;

namespace Project1.Application.Absent.Commands.CreateAbsent
{
    public class CreateAbsentHandler : IRequestHandler<CreateAbsentCommand, CreateAbsentResponse>
    {
        private readonly IAbsentRepository _repository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAbsentHandler"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mediator"></param>
        public CreateAbsentHandler(IAbsentRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;

        }
        /// <summary>
        /// Handle create absent command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<CreateAbsentResponse> Handle(CreateAbsentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var absent = new Models.Absent
            {
                UserId = command.UserId,
                StartDate = command.Body.StartDate,
                EndDate = command.Body.EndDate,
                Description = command.Body?.Description,
            };

            Models.Absent createdAbsent = await _repository
                .CreateAbsentAsync(absent, cancellationToken);

            var eventPublishAbsent = new AbsentLogicEvent(createdAbsent);
            await _mediator.Publish(eventPublishAbsent, cancellationToken);

            return new CreateAbsentResponse
            {
                Absent = absent
            };
        }
    }
}
