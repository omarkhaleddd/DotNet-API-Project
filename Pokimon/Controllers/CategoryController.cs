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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public  CategoryController(ICategoryRepository categoryRepository , IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public async Task<IActionResult> GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200,Type = typeof(Category))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetCategory(int categoryId) 
        {
            var flag = await _categoryRepository.CategoryExists(categoryId);
            if (!flag) 
            { 
                return NotFound();
            }
            var Category = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategory(categoryId));
            if(!ModelState.IsValid) 
            {   
                return BadRequest(ModelState);
            }
            return Ok(Category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type =typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId)
        {
            var flag = await _categoryRepository.CategoryExists(categoryId);
            if (!flag)
            {
                return NotFound();
            }
            var pokemons = _mapper.Map<List<PokemonDto>>(await _categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            if(category == null)
            {
                return BadRequest(ModelState);
            }
            var check = await _categoryRepository.GetCategories();

            // n2asmha 3la two steps
            var checks = check.Where(c => c.Name.Trim().ToUpper() == category.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (checks != null)
            {
                ModelState.AddModelError("","Data already exists!");
                return StatusCode(422,ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var categoryMap = _mapper.Map<Category>(category);
            var flag = await _categoryRepository.CreateCategory(categoryMap);
            if(!flag) 
            {
                ModelState.AddModelError("", "Something went wrong ");
                StatusCode(500,ModelState);
            }
            return Ok(category);
        }
        [HttpPut("{itemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int itemId, [FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }
            if (itemId != category.Id)
            {
                return BadRequest(ModelState);
            }
            var flag2 = await _categoryRepository.CategoryExists(itemId);
            if (!flag2)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(category);
            var flag = await _categoryRepository.UpdateCategory(categoryMap);
            if (!flag)
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
        public async Task<IActionResult> DeleteCategory(int itemId)
        {
            var flag = await _categoryRepository.CategoryExists(itemId);
            if (!flag)
            {
                return NotFound();
            }
            var itemToDelete = await _categoryRepository.GetCategory(itemId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var flag2 = await _categoryRepository.DeleteCategory(itemToDelete);
            if (!flag2)
            {
                ModelState.AddModelError("", "something went wrong");
            }
            return NoContent();
        }
    }
}
