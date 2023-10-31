using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<bool> CreatePokemon(Pokemon pokemon)
        {
            await _context.AddAsync(pokemon);
            return await Save();

        }

        public async Task<bool> DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return await Save();
        }

        public async Task<Pokemon> GetPokemon(int id)
        {
            return await _context.Pokemon.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            return await _context.Pokemon.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemons() 
        {
            return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }
        public async Task<bool> PokemonExists(int id) 
        {
            return await _context.Pokemon.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> Save()
        {
           var saved = await _context.SaveChangesAsync();
           return saved > 0 ? true : false;
        }

        public async Task<bool> UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return await Save();
        }
    }
}
