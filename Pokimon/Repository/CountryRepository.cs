using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context )
        {
            _context = context;
        }

        public async Task<bool> CountryExists(int id)
        {
            return await _context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _context.AddAsync(country);
            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            _context.Remove(country);
            return await Save();
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _context.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByOwner(int ownerId)
        {
               return await _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersFromCountry(int countryId)
        {
            return await _context.Owners.Where(c => c.Id == countryId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            _context.Update(country);
            return await Save();
        }
    }
}
