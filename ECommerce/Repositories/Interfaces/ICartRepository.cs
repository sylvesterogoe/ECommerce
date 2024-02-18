using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task AddCartAsync(Cart cart);
        Cart? GetCartById(int cartId);
        Cart? GetCartByUserId(int userId);

    }
}
