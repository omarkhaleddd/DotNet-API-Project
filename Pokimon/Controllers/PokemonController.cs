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
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository , IMapper mapper, IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(await _pokemonRepository.GetPokemons());
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int id)
        {
            if(!await _pokemonRepository.PokemonExists(id)) {
              return NotFound();
            }
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromBody]PokemonDto pokemon)
        {
            if(pokemon == null)
            {
                return BadRequest(ModelState);
            }
            var check = await _pokemonRepository.GetPokemons();
                
            var check2 =  check
                .Where(p => p.Name.Trim().ToUpper() == pokemon.Name.TrimEnd().ToUpper())
                .FirstOrDefault(); 
            if (check2 != null)
            {
                ModelState.AddModelError("", "data already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);

            if (!await _pokemonRepository.CreatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok(pokemon);
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon (int itemId , [FromBody] PokemonDto pokemon)
        {   
            if(pokemon == null)
            {
                return BadRequest(ModelState);
            }
            if(itemId != pokemon.Id)
            {
                return BadRequest(ModelState);
            }
            if(!await _pokemonRepository.PokemonExists(itemId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!await _pokemonRepository.UpdatePokemon(pokemonMap))
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
        public async Task<IActionResult> DeletePokemon(int itemId)
        {
            if (!await _pokemonRepository.PokemonExists(itemId))
            {
                return NotFound();
            }
            var itemToDelete = await _pokemonRepository.GetPokemon(itemId);
            var reviewsToDelete = await _reviewRepository.GetReviewsOfPokemon(itemId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "something went wrong");
            }
            if (!await _pokemonRepository.DeletePokemon(itemToDelete))
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }
    }
}
