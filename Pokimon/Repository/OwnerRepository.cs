using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Owner> GetOwner(int ownerId)
        {
            return await _context.Owners.Where(o=>o.Id == ownerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwners()
        {
            return await _context.Owners.OrderBy(o=>o.Id).ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnerByPokemon(int pokeId)
        {
           return await _context.PokimonOwner.Where(p => p.Pokemon.Id == pokeId).Select(o=> o.Owner).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId)
        {
            return await _context.PokimonOwner.Where(o => o.Owner.Id == ownerId).Select( p => p.Pokemon ).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _context.Owners.AnyAsync(o=> o.Id == ownerId);
        }
        public async Task<bool> CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return await Save();
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return await Save();
        }
    }
}
