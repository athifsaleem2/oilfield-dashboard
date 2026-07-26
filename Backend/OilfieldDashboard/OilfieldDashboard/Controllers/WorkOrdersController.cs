using MediatR;
using Microsoft.AspNetCore.Mvc;
using OilfieldDashboard.Application.Features.WorkOrders.Commands.CreateWorkOrder;
using OilfieldDashboard.Application.Features.WorkOrders.Commands.UpdateWorkOrder;
using OilfieldDashboard.Application.Features.WorkOrders.Commands.DeleteWorkOrder;
using OilfieldDashboard.Application.Features.WorkOrders.Queries.GetAllWorkOrders;

namespace OilfieldDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkOrderDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllWorkOrdersQuery()));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateWorkOrderCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateWorkOrderCommand command)
        {
            if (id != command.Id) return BadRequest("Route id and command id do not match.");
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteWorkOrderCommand(id));
            return NoContent();
        }
    }
}