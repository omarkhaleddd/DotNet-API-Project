using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
           _context.Add(category);
           return await Save();
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _context.Remove(category);
            return await Save();
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId)
        {
            return await _context.PokimonCategories.Where(c => c.CategoryId == categoryId).Select(c => c.Pokemon).ToListAsync();
        }

        public async Task<bool> Save()
        {
           var save = await _context.SaveChangesAsync();
            return save > 0 ? true : false; 
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _context.Update(category);
            return await Save();
        }
    }
}
