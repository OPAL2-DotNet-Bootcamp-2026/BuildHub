using Backend.Models;
using Backend.Models.Dtos;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>Lists every review.</summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetAll()
        {
            return Ok(await _reviewService.GetAllAsync());
        }

        /// <summary>Gets one review by id.</summary>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponse>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            return review is null ? NotFound() : Ok(review);
        }

        /// <summary>
        /// Rates the vendor behind a Completed agreement. The reviewer must be that
        /// agreement's homeowner; the vendor is read from the agreement, not the request.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Homeowner))]
        [HttpPost]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReviewResponse>> Create(CreateReviewRequest request)
        {
            var created = await _reviewService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.ReviewId }, created);
        }

        /// <summary>Changes the rating or comment and recomputes the vendor's average.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponse>> Update(int id, UpdateReviewRequest request)
        {
            var updated = await _reviewService.UpdateAsync(id, request);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Deletes a review and recomputes the vendor's average.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _reviewService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
