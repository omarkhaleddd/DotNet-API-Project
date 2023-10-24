using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> GetCountryByOwner(int ownerId);

        Task<Country> GetCountry(int id);
        Task<ICollection<Country>> GetCountries();
        Task<ICollection<Owner>> GetOwnersFromCountry(int countryId);
        Task<bool> CountryExists(int id);
        Task<bool> CreateCountry (Country country);
        Task<bool> Save();
        Task<bool> DeleteCountry(Country country);
        Task<bool> UpdateCountry(Country country);


    }
}
