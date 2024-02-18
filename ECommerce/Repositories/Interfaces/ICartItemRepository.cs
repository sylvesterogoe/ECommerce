using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        CartItem? GetCartItem(int cartItemId);
        Task<CartItem> UpdateCartItemQuantity(CartItem cartItem, int Quantity);
        Task AddCartItem(Cart cart, CartItem cartItem);
        Task DeleteCartItem(CartItem cartItem);
        IQueryable<CartItem> CreateQuery();
    }
}