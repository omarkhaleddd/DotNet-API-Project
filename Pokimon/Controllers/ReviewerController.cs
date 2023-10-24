using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokimon.Dto;
using Pokimon.Interfaces;
using Pokimon.Models;
using Pokimon.Repository;

namespace Pokimon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository , IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200 , Type = typeof(IEnumerable<Reviewer>) )]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>> (await _reviewerRepository.GetReviewers());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }
        [HttpGet("reviewerId")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            var flag =await _reviewerRepository.ReviewerExists(reviewerId);
            if(!flag)
            {
                return NotFound(); 
            }
            var reviewer = _mapper.Map<ReviewerDto>( await _reviewerRepository.GetReviewer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerId)
        {
            var flag = await _reviewerRepository.ReviewerExists(reviewerId);
            if (!flag)
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewerRepository.GetReviewsByReviewer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody]ReviewerDto reviewer)
        {
            if(reviewer == null)
            {
                return BadRequest(ModelState);
            }
            var check = await _reviewerRepository.GetReviewers();
                            
            var check2=check
                .Where(r => r.LastName.Trim().ToUpper() == reviewer.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if(check2 != null)
            {
                ModelState.AddModelError("", "Data already exists!");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(reviewer);
            var flag = await _reviewerRepository.CreateReviewer(reviewerMap);
            if (!flag)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }
            return Ok(reviewer);
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int itemId, [FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null)
            {
                return BadRequest(ModelState);
            }
            if (itemId != reviewer.Id)
            {
                return BadRequest(ModelState);
            }
            var flag = await _reviewerRepository.ReviewerExists(itemId);
            if (!flag)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(reviewer);
            var flag2 =await _reviewerRepository.UpdateReviewer(reviewerMap);
            if (!flag2)
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int itemId)
        {
            var flag =await _reviewerRepository.ReviewerExists(itemId);
            if (!flag)
            {
                return NotFound();
            }
            var itemToDelete =await _reviewerRepository.GetReviewer(itemId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var flag2 =await _reviewerRepository.DeleteReviewer(itemToDelete);
            if (!flag2)
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }

    }
}
