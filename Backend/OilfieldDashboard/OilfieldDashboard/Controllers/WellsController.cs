using MediatR;
using Microsoft.AspNetCore.Mvc;
using OilfieldDashboard.Application.Features.Wells.Commands.CreateWell;
using OilfieldDashboard.Application.Features.Wells.Commands.UpdateWell;
using OilfieldDashboard.Application.Features.Wells.Commands.DeleteWell;
using OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;
using OilfieldDashboard.Application.Features.Wells.Queries.GetWellById;

namespace OilfieldDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WellsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WellsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<WellDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllWellsQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WellDto>> GetById(int id)
        {
            var well = await _mediator.Send(new GetWellByIdQuery(id));
            return well is null ? NotFound() : Ok(well);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateWellCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateWellCommand command)
        {
            if (id != command.Id) return BadRequest("Route id and command id do not match.");
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWellCommand(id));
            return NoContent();
        }
    }
}