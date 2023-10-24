using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface ICategoryRepository
    {
         Task<ICollection<Category>> GetCategories();
         Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
         Task<bool> CategoryExists(int id);
         Task<bool> CreateCategory(Category category);
         Task<bool> Save();
         Task<bool> DeleteCategory(Category category);
         Task<bool> UpdateCategory(Category category);
    }
}
