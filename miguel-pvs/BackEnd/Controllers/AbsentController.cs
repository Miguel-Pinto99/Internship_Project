using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Models;
using Project1.Data;
using Project1.Persistance;
using MQTTnet.Client;
using MQTTnet;
using Newtonsoft.Json;
using Project1.Infrastructure;
using System.ComponentModel;
using MediatR;
using Project1.Application.Absent.Commands.CreateAbsent;
using Project1.Application.Absent.Queries.GetAllAbsent;
using Project1.Application.Absent.Commands.EditAbsent;
using Project1.Application.Absent.Queries.GetAbsent;
using Project1.Application.Absent.Commands.DeleteAbsent;
using Project1.Application.ApplicationUsers.Queries.EditWorkPatterm;
using Project1.Application.Absent.Queries.GetAbsentByIdById;

namespace Project1.Controllers
{

    [ApiController]
    [Route(template: "api/v1/employee-absent")]

    public class AbsentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AbsentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(int id, [FromBody] CreateAbsentCommandBody body, CancellationToken cancellationToken)
        {
            var command = new CreateAbsentCommand(id, body);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.Absent);
            }
            else
            {
                return BadRequest(response.Absent);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAbsentAsync(CancellationToken cancellationToken)
        {
            var command = new GetAllAbsentCommand();
            var response = await _mediator.Send(command, cancellationToken);
            if (response != null)
            {
                return Ok(response.ListAbsent);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(Guid id, [FromBody] EditAbsentCommandBody body, CancellationToken cancellationToken)
        {
            var command = new EditAbsentCommand(id, body);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.Absent);
            }
            else
            {
                return BadRequest(response.Absent);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetAbsentCommand(id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.Absent);
            }
            else
            {
                return BadRequest(response.Absent);
            }
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var command = new GetAbsentByIdCommand(userId);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.ListAbsent);
            }
            else
            {
                return BadRequest(response.ListAbsent);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteAbsentCommand(id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response != null)
            {
                return Ok(response.Absent);
            }
            else
            {
                return BadRequest(response.Absent);
            }
        }
    }
}
