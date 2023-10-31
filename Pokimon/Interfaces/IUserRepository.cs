using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUser (User user);
        public Task<ICollection<User>> GetUsers();
        public Task<User> GetUserByEmail(string email);
        public Task<bool> CreateUser (User user);
        public Task<bool> UserExists (User user);
        public Task<bool> Save();
    }
}
