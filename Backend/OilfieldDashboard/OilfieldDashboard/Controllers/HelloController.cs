using Microsoft.AspNetCore.Mvc;

namespace OilfieldDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        /// <summary>
        /// Health check / hello-world endpoint.
        /// Returns a simple status object to confirm the API is live.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Oilfield Dashboard API is live!",
                status = "healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
