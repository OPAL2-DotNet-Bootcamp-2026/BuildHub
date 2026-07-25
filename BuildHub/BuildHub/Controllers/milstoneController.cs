using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    public class MilstoneController
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



          




        }



    }
}
