
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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(await _countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            var flag = await _countryRepository.CountryExists(countryId); 
            if (!flag)
            {
                return NotFound();
            }
            var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }
        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto country)
        {
            if (country == null)
            {
                return BadRequest(ModelState);
            }
            var check =await _countryRepository.GetCountries();
            var check2 = check.Where(c => c.Name.Trim().ToUpper() == country.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (check2 != null)
            {
                ModelState.AddModelError("", "Data already exists!");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = _mapper.Map<Country>(country);
            var flag = await _countryRepository.CreateCountry(countryMap);
            if (!flag)
            {
                ModelState.AddModelError("", "something went wrong !");
                return StatusCode(500, ModelState);
            }
            return Ok(country);
        }
        [HttpDelete("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int itemId)
        {
            var flag = await _countryRepository.CountryExists(itemId); 
            if (!flag){
                return NotFound();
            }
            var itemToDelete =await  _countryRepository.GetCountry(itemId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var flag2 =await _countryRepository.DeleteCountry(itemToDelete);
            if (!flag2)
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry(int itemId, [FromBody] CountryDto country)
        {
            if (country == null)
            {
                return BadRequest(ModelState);
            }
            if (itemId != country.Id)
            {
                return BadRequest(ModelState);
            }
            var flag = await _countryRepository.CountryExists(itemId);
            if (!flag)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = _mapper.Map<Country>(country);
            var flag2 =await  _countryRepository.UpdateCountry(countryMap);
            if (!flag2)
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
