using MediatR;
using Microsoft.AspNetCore.Mvc;
using OilfieldDashboard.Application.Features.Alerts.GetAllAlerts;
using OilfieldDashboard.Application.Features.Alerts.ResolveAlert;
using OilfieldDashboard.Application.Interfaces.Features.Alerts.GetAllAlerts;

namespace OilfieldDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlertsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllAlertsQuery()));
        }

        [HttpPut("{id:int}/resolve")]
        public async Task<IActionResult> Resolve(int id)
        {
            await _mediator.Send(new ResolveAlertCommand(id));
            return NoContent();
        }
    }
}