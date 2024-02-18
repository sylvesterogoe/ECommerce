using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User newUser);
    }
}
