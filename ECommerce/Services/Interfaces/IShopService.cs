using ECommerce.Models;

namespace ECommerce.Services.Interfaces
{
    public interface IShopService
    {
        Task AddUser(User newUser);

        Task AddCart(Cart cart);

        Task<CartItem> AddItemToCartAsync(CartItem cartItem, int userId);

        List<CartItem> GetAllCartItems(int cartId);

        CartItem? GetCartItem(int cartItemId);

        Task<CartItem?> RemoveCartItem(int cartItemId);
    }
}
