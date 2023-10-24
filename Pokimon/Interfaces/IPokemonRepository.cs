using AutoMapper.Configuration.Conventions;
using Pokimon.Models;
using System.Diagnostics.Contracts;

namespace Pokimon.Interfaces
{
    public interface IPokemonRepository 
    {
         Task<ICollection<Pokemon>> GetPokemons();
         Task<Pokemon> GetPokemon(int id);
         Task<Pokemon> GetPokemon(string name);
         Task<bool> PokemonExists(int id);
         Task<bool> CreatePokemon(Pokemon pokemon);
         Task<bool> Save();
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> UpdatePokemon(Pokemon pokemon);
    }
}
