using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Project1.Persistance;
using Project1.Infrastructure;
using MediatR;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;
using Project1.Application.WorkPatterns.Commands.DeleteWorkPattern;
using Project1.Application.WorkPatterns.Commands.EditWorkPattern;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;
using Project1.Application.WorkPatterns.Queries.GetAllWorkPattern;
using Project1.Application.WorkPatterns.Queries.GetWorkPattern;

namespace Project1.Controllers
{
    [ApiController]
    [Route(template: "api/v1/work-patterns")]
    public class WorkPatternController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWorkPatternRepository _repository;
        private readonly IUnsService _unsService;
        private readonly IApplicationUsersRepository _workPatternRepository;
        private readonly IMediator _mediator;

        public WorkPatternController(AppDbContext context,
            IWorkPatternRepository repository,
            IUnsService unsService,
            IMediator mediator,
            IApplicationUsersRepository workPatternRepository)
        {
            _context = context;
            _repository = repository;
            _unsService = unsService;
            _mediator = mediator;
            _workPatternRepository = workPatternRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(int id, [FromBody] CreateWorkPatternCommandBody body, CancellationToken cancellationToken)
        {

            var command = new CreateWorkPatternCommand(id, body);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.WorkPattern);
            }
            else
            {
                return BadRequest(response.WorkPattern);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkPatternsAsync(CancellationToken cancellationToken)
        {
            var command = new GetAllWorkPatternCommand();
            var response = await _mediator.Send(command, cancellationToken);
            if (response != null)
            {
                return Ok(response.listWorkPattern);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(Guid id, [FromBody] EditWorkPatternCommandBody body, CancellationToken cancellationToken)
        {
            var command = new EditWorkPatternCommand(id, body);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.WorkPattern);
            }
            else
            {
                return BadRequest(response.WorkPattern);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetWorkPatternCommand(id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.WorkPattern);
            }
            else
            {
                return BadRequest(response.WorkPattern);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteWorkPatternCommand(id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.WorkPattern);
            }
            else
            {
                return BadRequest(response.WorkPattern);
            }
        }
    }
}

