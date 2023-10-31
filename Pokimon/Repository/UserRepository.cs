using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<bool> CreateUser(User user)
        {
            await _context.users.AddAsync(user);
            return await Save();
        }

        public async Task<User> GetUser(User user)
        {
            return await _context.users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UserExists(User user)
        {
            return await _context.users.AnyAsync(u => u.Id == user.Id);
        }
    }
}
