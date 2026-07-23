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



            [HttpPost("milestone")]
            public IActionResult Milestone([FromBody] Milestone dto)
            {
                MilestoneDto created = milestoneService.Milestone(dto);

                if (created == null)
                {
                    return BadRequest(new { message = "Alredy registerd." });
                    return Ok (created);
                        
                }

            }




        }



    
}
