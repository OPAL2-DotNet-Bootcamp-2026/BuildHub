using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>Lists every job.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobResponse>>> GetAll()
        {
            return Ok(await _jobService.GetAllAsync());
        }

        /// <summary>Gets one job by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(JobResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobResponse>> GetById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            return job is null ? NotFound() : Ok(job);
        }

        /// <summary>Posts a job. It starts Open.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(JobResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobResponse>> Create(CreateJobRequest request)
        {
            var created = await _jobService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.JobId }, created);
        }

        /// <summary>Updates the details of the request. Status is not editable here.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(JobResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobResponse>> Update(int id, UpdateJobRequest request)
        {
            var updated = await _jobService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes a job none of whose offers became an agreement.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _jobService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
