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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            var flag = await _ownerRepository.OwnerExists(ownerId);
            if (!flag)
            {
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }


        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByOwner(int ownerId)
        {
            var flag= await _ownerRepository.OwnerExists(ownerId);
            if (!flag)
            {
                return NotFound();
            }
            var owner = _mapper.Map<List<PokemonDto>>(
                await _ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var check = await _ownerRepository.GetOwners();
            var check2 = check.Where(o => o.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault(); ;
            if (check2 != null)
            {
                ModelState.AddModelError("", "Data already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = await _countryRepository.GetCountry(countryId);
            if (!await _ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }
            return Ok(ownerCreate);
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOwner(int itemId, [FromBody] OwnerDto owner)
        {
            if (owner == null)
            {
                return BadRequest(ModelState);
            }
            if (itemId != owner.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _ownerRepository.OwnerExists(itemId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(owner);
            if (!await _ownerRepository.UpdateOwner(ownerMap))
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
        public async Task<IActionResult> DeleteOwner(int itemId)
        {
            var flag = await _ownerRepository.OwnerExists(itemId) ;
            if (!flag)
            {
                return NotFound();
            }
            var itemToDelete =await  _ownerRepository.GetOwner(itemId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var flag2 = await _ownerRepository.DeleteOwner(itemToDelete); 
            if (!flag2)
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }
    }
}
