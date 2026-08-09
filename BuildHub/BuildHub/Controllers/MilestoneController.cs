using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("Milestone")]
    public class MilestoneController : ControllerBase
    {
        private MilestoneService milestoneService;

        public MilestoneController(MilestoneService _milestoneService)
        {
            milestoneService = _milestoneService;
        }


        // NOTE: still a stub - it validates and echoes 200 but persists nothing.
        [HttpPost("milestone")]
        public IActionResult Milestone([FromBody] CreateMilestoneDto dto)
        {
            return Ok();
        }
    }
}
