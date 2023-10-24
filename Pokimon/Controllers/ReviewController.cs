using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokimon.Dto;
using Pokimon.Helper;
using Pokimon.Interfaces;
using Pokimon.Models;
using Pokimon.Repository;

namespace Pokimon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;

        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int id)
        {
            if (!await _reviewRepository.ReviewExists(id))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDto>(await _reviewRepository.GetReview(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewOfPokemon(int pokeId)
        {
            if (!await _reviewRepository.ReviewExists(pokeId))
            {
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviewsOfPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId,[FromQuery]int pokeId, [FromForm]  ReviewDto review)
        {
            if (review == null)
            {
                return BadRequest(ModelState);
            }
            var check =await _reviewRepository.GetReviews();
            var check2 = check
                .Where(r => r.Title.Trim().ToUpper() == review.Title.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (check2 != null)
            {
                ModelState.AddModelError("", "Data already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(review);
            reviewMap.ImageUrl = FileUploader.UploadFile(reviewMap.Image, "Files");

            reviewMap.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokeId);


            if (!await _reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something Went Wrong!");
                return StatusCode(500, ModelState);
            }
            return Ok(review);
        }
        [HttpDelete("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReview(int itemId)
        {
            if (!await _reviewRepository.ReviewExists(itemId))
            {
                return NotFound();
            }
            var itemToDelete = await _reviewRepository.GetReview(itemId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _reviewRepository.DeleteReview(itemToDelete))
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReview(int itemId, [FromBody] ReviewDto review)
        {
            if (review == null)
            {
                return BadRequest(ModelState);
            }
            if (itemId != review.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _reviewRepository.ReviewExists(itemId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(review);
            if (!await _reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
