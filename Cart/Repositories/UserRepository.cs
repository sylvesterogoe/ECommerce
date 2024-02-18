using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;

namespace ECommerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
