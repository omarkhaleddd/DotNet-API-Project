using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwners();
        Task<Owner> GetOwner(int ownerId); 
        Task<ICollection<Owner>> GetOwnerByPokemon(int pokeId);
        Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> Save();
        Task<bool> DeleteOwner(Owner owner);
        Task<bool> UpdateOwner(Owner owner);

    }

}
